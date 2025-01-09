USE ModelDatabase;
GO

-- Tabela Administrador
INSERT INTO Administrador (ID, Nome, Email, PalavraPasse)
VALUES
(1, 'Admin1', 'admin1@example.com', 'password123'),
(2, 'Admin2', 'admin2@example.com', 'password456');

-- Tabela Instituição
INSERT INTO Instituicao (ID, Nome, NIF, NumeroAssociacao, Email, Morada, Creditos, PalavraPasse)
VALUES
(1, 'Instituicao A', '123456789', 101, 'contacto@insta.com', 'Rua 1', 1000, 'pass123'),
(2, 'Instituicao B', '987654321', 102, 'contacto@instb.com', 'Rua 2', 2000, 'pass456');

-- Tabela Colaborador
INSERT INTO Colaborador (ID, Nome, Email, DataNascimento, InstituicaoID, PalavraPasse)
VALUES
(1, 'Colaborador A', 'colabA@example.com', '1980-01-01', 1, 'colab123'),
(2, 'Colaborador B', 'colabB@example.com', '1990-01-01', 2, 'colab456');

-- Tabela Manual
INSERT INTO Manual (ID, Capa, Nome, Descricao)
VALUES
(1, 'manual1.jpg', 'Manual A', 'Descrição do Manual A'),
(2, 'manual2.jpg', 'Manual B', 'Descrição do Manual B');

-- Tabela Manual da Instituição
INSERT INTO ManualInstituicao (IDInstituicao, IDManual)
VALUES
(1, 1),
(2, 2);

-- Tabela Página
INSERT INTO Pagina (ID, ImagemAlusiva, Numeracao, ManualAssociado)
VALUES
(1, 'pagina1.jpg', 1, 1),
(2, 'pagina2.jpg', 2, 2);

-- Tabela Produto
INSERT INTO Produto (ID, Nome, Descricao, Custo, IdadeMinima, ImagemAlusiva, ManualAssociado)
VALUES
(1, 'Produto A', 'Descrição do Produto A', 50, 18, 'produtoA.jpg', 1),
(2, 'Produto B', 'Descrição do Produto B', 100, 21, 'produtoB.jpg', 2);

-- Tabela Peça
INSERT INTO Peca (ID, Nome, ImagemAlusiva, Quantidade)
VALUES
(1, 'Peça A', 'pecaA.jpg', 10),
(2, 'Peça B', 'pecaB.jpg', 5);

-- Tabela Peça por Produto
INSERT INTO PecaProduto (IDPeca, IDProduto, Quantidade)
VALUES
(1, 1, 2),
(2, 2, 3);

-- Tabela Compra
INSERT INTO Compra (NumeroCompra, DataCompra, EnderecoEntrega, ProdutoAssociado, InstituicaoID)
VALUES
(1, '2024-12-01', 'Rua de Entrega 1', 1, 1),
(2, '2024-12-02', 'Rua de Entrega 2', 2, 2);
GO
