CREATE PROCEDURE VerificarIniciarSessao
    @Email VARCHAR(45),
    @PalavraPasse VARCHAR(250),
    @Nome VARCHAR(45) OUTPUT,
    @Role VARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Inicializar as variáveis de saída com NULL
    SET @Nome = NULL;
    SET @Role = NULL;

    -- Verificar na tabela Administrador
    IF EXISTS (
        SELECT 1
        FROM Administrador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse
    )
    BEGIN
        SELECT 
            @Nome = Nome, 
            @Role = 'Admin'
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
            @Nome = Nome, 
            @Role = 'Institution'
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
            @Nome = Nome, 
            @Role = 'Collaborator'
        FROM Colaborador
        WHERE Email = @Email AND PalavraPasse = @PalavraPasse;

        RETURN;
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
        INSERT INTO ManualInstituicao (IDInstituicao, IDManual)
        VALUES (@codInstituicao, @ManualAssociado);

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
