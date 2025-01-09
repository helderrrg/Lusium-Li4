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
