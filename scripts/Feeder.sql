-- Tabela Administrador
INSERT INTO Administrador (Nome, Email, PalavraPasse)
VALUES
('Admin1', 'admin1@example.com', 'password123'),
('Admin2', 'admin2@example.com', 'password456'),
('Admin3', 'admin3@example.com', 'password789'),
('Admin4', 'admin4@example.com', 'password321');

-- Tabela Instituição
INSERT INTO Instituicao (Nome, NIF, NumeroAssociacao, Email, Morada, Creditos, PalavraPasse)
VALUES
('Instituicao A', '123456789', 101, 'contacto@insta.com', 'Rua 1', 1000, 'pass123'),
('Instituicao B', '987654321', 102, 'contacto@instb.com', 'Rua 2', 2000, 'pass456'),
('Instituicao C', '111222333', 103, 'contacto@instc.com', 'Rua 3', 3000, 'pass789'),
('Instituicao D', '444555666', 104, 'contacto@instd.com', 'Rua 4', 4000, 'pass321');

-- Tabela Colaborador
INSERT INTO Colaborador (Nome, Email, DataNascimento, InstituicaoID, PalavraPasse)
VALUES
('Colaborador A', 'colabA@example.com', '1980-01-01', 1, 'colab123'),
('Colaborador B', 'colabB@example.com', '1990-01-01', 2, 'colab456'),
('Colaborador C', 'colabC@example.com', '1985-06-15', 3, 'colab789'),
('Colaborador D', 'colabD@example.com', '1992-09-10', 4, 'colab321');

-- Tabela Manual
INSERT INTO Manual (Capa, Nome, Descricao)
VALUES
('manual1.jpg', 'Manual A', 'Descrição do Manual A'),
('manual2.jpg', 'Manual B', 'Descrição do Manual B'),
('manual3.jpg', 'Manual C', 'Descrição do Manual C'),
('manual4.jpg', 'Manual D', 'Descrição do Manual D');

-- Tabela Manual da Instituição
--INSERT INTO ManualInstituicao (IDInstituicao, IDManual)
--VALUES
--(1, 1),
--(2, 2),
--(3, 3),
--(4, 4);

-- Tabela Página
INSERT INTO Pagina (ImagemAlusiva, Numeracao, ManualAssociado)
VALUES
('pagina1.jpg', 1, 1),
('pagina2.jpg', 2, 2),
('pagina3.jpg', 1, 3),
('pagina4.jpg', 2, 4);

-- Tabela Produto
INSERT INTO Produto (Nome, Descricao, Custo, IdadeMinima, ImagemAlusiva, ManualAssociado)
VALUES
('Produto A', 'Descrição do Produto A', 50, 18, 'produtoA.jpg', 1),
('Produto B', 'Descrição do Produto B', 100, 21, 'produtoB.jpg', 2),
('Produto C', 'Descrição do Produto C', 150, 16, 'produtoC.jpg', 3),
('Produto D', 'Descrição do Produto D', 200, 25, 'produtoD.jpg', 4);

-- Tabela Peça
INSERT INTO Peca (Nome, ImagemAlusiva, Quantidade)
VALUES
('Peça A', 'pecaA.jpg', 10),
('Peça B', 'pecaB.jpg', 5),
('Peça C', 'pecaC.jpg', 8),
('Peça D', 'pecaD.jpg', 12);

-- Tabela Peça por Produto
INSERT INTO PecaProduto (IDPeca, IDProduto, Quantidade)
VALUES
(1, 1, 2),
(2, 2, 3),
(3, 3, 4),
(4, 4, 1);

-- Tabela Compra
--INSERT INTO Compra (DataCompra, EnderecoEntrega, ProdutoAssociado, InstituicaoID)
--VALUES
--('2024-12-01', 'Rua de Entrega 1', 1, 1),
--('2024-12-02', 'Rua de Entrega 2', 2, 2),
--('2025-01-01', 'Rua de Entrega 3', 3, 3),
--('2025-01-02', 'Rua de Entrega 4', 4, 4);
GO
