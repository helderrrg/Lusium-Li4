-- Desativar restrições temporariamente para evitar problemas de dependência
ALTER TABLE ManualInstituicao NOCHECK CONSTRAINT ALL;
ALTER TABLE Pagina NOCHECK CONSTRAINT ALL;
ALTER TABLE Colaborador NOCHECK CONSTRAINT ALL;
ALTER TABLE PecaProduto NOCHECK CONSTRAINT ALL;
ALTER TABLE Compra NOCHECK CONSTRAINT ALL;

-- Apagar tabelas, caso existam
IF OBJECT_ID('ManualInstituicao', 'U') IS NOT NULL DROP TABLE ManualInstituicao;
IF OBJECT_ID('Pagina', 'U') IS NOT NULL DROP TABLE Pagina;
IF OBJECT_ID('Colaborador', 'U') IS NOT NULL DROP TABLE Colaborador;
IF OBJECT_ID('Compra', 'U') IS NOT NULL DROP TABLE Compra;
IF OBJECT_ID('PecaProduto', 'U') IS NOT NULL DROP TABLE PecaProduto;
IF OBJECT_ID('Produto', 'U') IS NOT NULL DROP TABLE Produto;
IF OBJECT_ID('Peca', 'U') IS NOT NULL DROP TABLE Peca;
IF OBJECT_ID('Manual', 'U') IS NOT NULL DROP TABLE Manual;
IF OBJECT_ID('Instituicao', 'U') IS NOT NULL DROP TABLE Instituicao;
IF OBJECT_ID('Administrador', 'U') IS NOT NULL DROP TABLE Administrador;
GO
