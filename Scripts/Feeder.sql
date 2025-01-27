-- Tabela Administrador
INSERT INTO Administrador (Nome, Email, PalavraPasse, Validado)
VALUES
('Admin1', 'admin1@example.com', 'password123#', 1),
('Admin2', 'admin2@example.com', 'password456#', 1),
('Admin3', 'admin3@example.com', 'password789#', 1),
('Admin4', 'admin4@example.com', 'password321#', 1);

-- Tabela Instituição
INSERT INTO Instituicao (Nome, NIF, NumeroAssociacao, Email, Morada, Creditos, PalavraPasse)
VALUES
('Instituicao A', '123456789', 101101, 'contacto@insta.com', 'Rua 1', 1000, 'password123#'),
('Instituicao B', '987654321', 102102, 'contacto@instb.com', 'Rua 2', 2000, 'password456#'),
('Instituicao C', '111222333', 103103, 'contacto@instc.com', 'Rua 3', 3000, 'password789#'),
('Instituicao D', '444555666', 104104, 'contacto@instd.com', 'Rua 4', 4000, 'password321#');

-- Tabela Colaborador
INSERT INTO Colaborador (Nome, Email, DataNascimento, InstituicaoID, PalavraPasse)
VALUES
('Colaborador A', 'colabA@example.com', '1980-01-01', 1, 'password123#'),
('Colaborador B', 'colabB@example.com', '1990-01-01', 2, 'password456#'),
('Colaborador C', 'colabC@example.com', '1985-06-15', 3, 'password789#'),
('Colaborador D', 'colabD@example.com', '1992-09-10', 4, 'password321#');

-- Tabela Manual
INSERT INTO Manual (Capa, Nome, Descricao)
VALUES
('/Products/Blast/blast-capa.png', 'Blast', 'Criado para destruir, mantido para salvar.'),
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
('/Products/Blast/Manual/blast-manual-1.png', 1, 1),
('/Products/Blast/Manual/blast-manual-2.png', 2, 1),
('/Products/Blast/Manual/blast-manual-3.png', 3, 1),
('/Products/Blast/Manual/blast-manual-4.png', 4, 1),
('/Products/Blast/Manual/blast-manual-5.png', 5, 1),
('/Products/Blast/Manual/blast-manual-6.png', 6, 1),
('/Products/Blast/Manual/blast-manual-7.png', 7, 1),
('/Products/Blast/Manual/blast-manual-8.png', 8, 1),
('/Products/Blast/Manual/blast-manual-9.png', 9, 1),
('/Products/Blast/Manual/blast-manual-10.png', 10, 1),
('/Products/Blast/Manual/blast-manual-11.png', 11, 1),
('/Products/Blast/Manual/blast-manual-12.png', 12, 1),
('/Products/Blast/Manual/blast-manual-13.png', 13, 1),
('/Products/Blast/Manual/blast-manual-14.png', 14, 1),
('/Products/Blast/Manual/blast-manual-15.png', 15, 1),
('/Products/Blast/Manual/blast-manual-16.png', 16, 1),
('/Products/Blast/Manual/blast-manual-17.png', 17, 1),
('pagina2.jpg', 2, 2),
('pagina3.jpg', 1, 3),
('pagina4.jpg', 2, 4);

-- Tabela Produto
INSERT INTO Produto (Nome, Descricao, Custo, IdadeMinima, ImagemAlusiva, ManualAssociado)
VALUES
('Blast', 'Criado para destruir, mantido para salvar.', 50, 18, '/Products/Blast/blast-capa.png', 1),
('Produto B', 'Descrição do Produto B', 100, 21, 'produtoB.jpg', 2),
('Produto C', 'Descrição do Produto C', 150, 16, 'produtoC.jpg', 3),
('Produto D', 'Descrição do Produto D', 200, 25, 'produtoD.jpg', 4);

-- Tabela Peça
INSERT INTO Peca (Nome, ImagemAlusiva, Quantidade)
VALUES
('Peça 1', '/Products/Blast/Pecas/blast-peca-1.png', 10),
('Peça 2', '/Products/Blast/Pecas/blast-peca-2.png', 10),
('Peça 3', '/Products/Blast/Pecas/blast-peca-3.png', 10),
('Peça 4', '/Products/Blast/Pecas/blast-peca-4.png', 10),
('Peça 5', '/Products/Blast/Pecas/blast-peca-5.png', 10),
('Peça 6', '/Products/Blast/Pecas/blast-peca-6.png', 10),
('Peça B', 'pecaB.jpg', 5),
('Peça C', 'pecaC.jpg', 8),
('Peça D', 'pecaD.jpg', 12);

-- Tabela Peça por Produto
INSERT INTO PecaProduto (IDPeca, IDProduto, Quantidade)
VALUES
(1, 1, 2),
(2, 1, 2),
(3, 1, 2),
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
