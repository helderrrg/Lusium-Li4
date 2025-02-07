CREATE PROCEDURE VerificarIniciarSessao
    @Email VARCHAR(45),
    @PalavraPasse VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar na tabela Administrador
    IF EXISTS (
        SELECT 1
        FROM Administrador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SELECT 
            ID,
            Nome,
			Email,
            'Admin' AS Role,
            Validado
        FROM Administrador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse;

        RETURN;
    END

    -- Verificar na tabela Instituicao
    IF EXISTS (
        SELECT 1
        FROM Instituicao
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SELECT 
            ID,
            Nome,
			Email,
            'Institution' AS Role
        FROM Instituicao
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse;

        RETURN;
    END

    -- Verificar na tabela Colaborador
    IF EXISTS (
        SELECT 1
        FROM Colaborador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SELECT 
            ID,
            Nome,
			Email,
            'Collaborator' AS Role
        FROM Colaborador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse;

        RETURN;
    END
END;
GO

CREATE PROCEDURE ValidaPP
    @Email VARCHAR(45),
    @PalavraPasse VARCHAR(250),
    @IsValid BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Inicializar a variável de saída
    SET @IsValid = 0;

    -- Verificar nas tabelas
    IF EXISTS (
        SELECT 1
        FROM Administrador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SET @IsValid = 1;
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM Instituicao
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SET @IsValid = 1;
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM Colaborador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SET @IsValid = 1;
        RETURN;
    END
END;
GO

CREATE PROCEDURE VerificarDisponibilidadeProduto
    @codProduto INT,
    @Disponivel BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Inicializa a variável de saída como 1 (disponível)
    SET @Disponivel = 1;

    -- Verifica se existe alguma peça com quantidade insuficiente
    IF EXISTS (
        SELECT 1
        FROM PecaProduto pp
        INNER JOIN Peca p ON pp.IDPeca = p.ID
        WHERE pp.IDProduto = @codProduto
          AND p.Quantidade < pp.Quantidade
    )
    BEGIN
        -- Define o valor de saída como 0 (indisponível)
        SET @Disponivel = 0;
    END
END;
GO

CREATE PROCEDURE VerificarSaldoInstituicao
    @codInstituicao INT,
    @codProduto INT,
    @SaldoSuficiente BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Inicializa a variável de saída como 1 (saldo suficiente)
    SET @SaldoSuficiente = 1;

    -- Verifica se o saldo da instituição é insuficiente
    IF EXISTS (
        SELECT Saldo
        FROM Instituicao
        WHERE ID = @codInstituicao
          AND Saldo < (
            SELECT Preco
            FROM Produto
            WHERE ID = @codProduto
          )
    )
    BEGIN
        -- Define o valor de saída como 0 (saldo insuficiente)
        SET @SaldoSuficiente = 0;
    END
END;
GO

CREATE PROCEDURE ProcessaCompra
    @codInstituicao INT,
    @codProduto INT,
    @EnderecoEntrega VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustoProduto INT;
    DECLARE @ManualAssociado INT;
    DECLARE @DataCompra DATE = GETDATE();

    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. Obter o custo total do produto e o manual associado
        SELECT @CustoProduto = Custo, 
               @ManualAssociado = ManualAssociado
        FROM Produto
        WHERE ID = @codProduto;

        -- Verificar se o produto foi encontrado
        IF @CustoProduto IS NULL OR @ManualAssociado IS NULL
        BEGIN
            THROW 50001, 'Produto não encontrado.', 1;
        END

        -- 2. Atualizar o saldo de créditos da instituição
        UPDATE Instituicao
        SET Creditos = Creditos - @CustoProduto
        WHERE ID = @codInstituicao;

        -- Verificar se a instituição foi encontrada
        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50002, 'Instituição não encontrada ou saldo insuficiente.', 1;
        END

        -- 3. Atualizar o stock geral diminuindo as peças constituintes do produto
        UPDATE p
        SET p.Quantidade = p.Quantidade - ppp.Quantidade
        FROM Peca p
        INNER JOIN PecaProduto ppp ON p.ID = ppp.IDPeca
        WHERE ppp.IDProduto = @codProduto;

        -- Verificar se houve impacto no stock
        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50003, 'Erro ao atualizar o stock. Verifique as peças associadas.', 1;
        END

        -- 4. Criar o registo da compra
        INSERT INTO Compra (DataCompra, EnderecoEntrega, ProdutoAssociado, InstituicaoID)
        VALUES (@DataCompra, @EnderecoEntrega, @codProduto, @codInstituicao);

        -- 5. Disponibilizar o manual do produto na listagem de manuais da instituição
        IF NOT EXISTS (
            SELECT 1
            FROM ManualInstituicao
            WHERE IDInstituicao = @codInstituicao
                AND IDManual = @ManualAssociado
        )
        BEGIN
            INSERT INTO ManualInstituicao (IDInstituicao, IDManual)
            VALUES (@codInstituicao, @ManualAssociado);
        END;

        -- Se tudo foi bem-sucedido, confirmar a transação
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Reverter a transação em caso de erro
        ROLLBACK TRANSACTION;

        -- Propagar o erro
        THROW;
    END CATCH
END;
GO



CREATE PROCEDURE ListarManuais
    @codUtilizador INT,
    @tipoUtilizador VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar tipo de utilizador
    IF @tipoUtilizador = 'Admin'
    BEGIN
        -- O administrador pode visualizar todos os manuais
        SELECT 
            m.ID, 
            m.Capa, 
            m.Nome, 
            m.Descricao
        FROM 
            Manual m;
    END
    ELSE IF @tipoUtilizador = 'Institution'
    BEGIN
        -- A instituição visualiza apenas os manuais associados
        SELECT 
            m.ID, 
            m.Capa, 
            m.Nome, 
            m.Descricao
        FROM 
            Manual m
        INNER JOIN 
            ManualInstituicao mi ON m.ID = mi.IDManual
        WHERE 
            mi.IDInstituicao = @codUtilizador;
    END
    ELSE IF @tipoUtilizador = 'Collaborator'
    BEGIN
        -- O colaborador visualiza apenas os manuais da sua instituição
        SELECT 
            m.ID, 
            m.Capa, 
            m.Nome, 
            m.Descricao
        FROM 
            Manual m
        INNER JOIN 
            ManualInstituicao mi ON m.ID = mi.IDManual
        INNER JOIN 
            Colaborador c ON mi.IDInstituicao = c.InstituicaoID
        WHERE 
            c.ID = @codUtilizador;
    END
    ELSE
    BEGIN
        -- Retornar NULL para sinalizar tipo inválido
        SELECT NULL AS ID, NULL AS Capa, NULL AS Nome, NULL AS Descricao;
    END
END;
GO

CREATE PROCEDURE CalcularCreditosDispendidos
    @InstituicaoID INT
AS
BEGIN
    -- Variável para armazenar o total de créditos dispendidos
    DECLARE @TotalCreditos INT;

    -- Somar os custos dos produtos associados às compras da instituição
    SELECT @TotalCreditos = SUM(p.Custo)
    FROM Compra c
    JOIN Produto p ON c.ProdutoAssociado = p.ID
    WHERE c.InstituicaoID = @InstituicaoID;

    -- Devolver o total de créditos dispendidos
    SELECT @TotalCreditos AS TotalCreditos;
END;
GO
