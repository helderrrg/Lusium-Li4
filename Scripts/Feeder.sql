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
-- Colaboradores da Instituição 1
('João Silva', 'joao.silva1@example.com', '1980-03-15', 1, 'JoaoSilva80#'),
('Ana Martins', 'ana.martins1@example.com', '1975-07-22', 1, 'AnaMartins75#'),
('Carlos Oliveira', 'carlos.oliveira1@example.com', '1990-11-10', 1, 'CarlosOliveira90#'),
('Maria Fernandes', 'maria.fernandes1@example.com', '1987-05-30', 1, 'MariaFernandes87#'),
('Pedro Sousa', 'pedro.sousa1@example.com', '1995-02-18', 1, 'PedroSousa95#'),

-- Colaboradores da Instituição 2
('José Santos', 'jose.santos2@example.com', '1978-09-12', 2, 'JoseSantos78#'),
('Carla Costa', 'carla.costa2@example.com', '1982-06-24', 2, 'CarlaCosta82#'),
('Fernando Almeida', 'fernando.almeida2@example.com', '1991-01-14', 2, 'FernandoAlmeida91#'),
('Sofia Lopes', 'sofia.lopes2@example.com', '1988-10-05', 2, 'SofiaLopes88#'),
('Rui Pereira', 'rui.pereira2@example.com', '1993-12-08', 2, 'RuiPereira93#'),

-- Colaboradores da Instituição 3
('Miguel Ribeiro', 'miguel.ribeiro3@example.com', '1983-04-21', 3, 'MiguelRibeiro83#'),
('Patrícia Gonçalves', 'patricia.goncalves3@example.com', '1992-03-17', 3, 'PatriciaGoncalves92#'),
('Bruno Carvalho', 'bruno.carvalho3@example.com', '1986-08-13', 3, 'BrunoCarvalho86#'),
('Rita Nogueira', 'rita.nogueira3@example.com', '1979-01-09', 3, 'RitaNogueira79#'),
('Tiago Matos', 'tiago.matos3@example.com', '1990-05-25', 3, 'TiagoMatos90#'),

-- Colaboradores da Instituição 4
('Luís Rodrigues', 'luis.rodrigues4@example.com', '1984-02-11', 4, 'LuisRodrigues84#'),
('Mariana Figueiredo', 'mariana.figueiredo4@example.com', '1989-07-30', 4, 'MarianaFigueiredo89#'),
('André Mendes', 'andre.mendes4@example.com', '1994-09-19', 4, 'AndreMendes94#'),
('Helena Moreira', 'helena.moreira4@example.com', '1981-11-03', 4, 'HelenaMoreira81#'),
('Fábio Rocha', 'fabio.rocha4@example.com', '1996-06-27', 4, 'FabioRocha96#');

-- Tabela Manual
INSERT INTO Manual (Capa, Nome, Descricao)
VALUES
('/Products/Blast/blast-capa.png', 'Blast', 'Criado para destruir, mantido para salvar.'),
('/Products/Charlie/charlie-capa.png', 'Charlie', 'Preferes que toque bateria?'),
('/Products/Tricky/tricky-capa.png', 'Tricky', 'Atenção... Lá vai a bola... Cesto!');

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
('/Products/Charlie/Manual/charlie-manual-1.png', 1, 2),
('/Products/Charlie/Manual/charlie-manual-2.png', 2, 2),
('/Products/Charlie/Manual/charlie-manual-3.png', 3, 2),
('/Products/Charlie/Manual/charlie-manual-4.png', 4, 2),
('/Products/Tricky/Manual/tricky-manual-1.png', 1, 3),
('/Products/Tricky/Manual/tricky-manual-2.png', 1, 3);

-- Tabela Produto
INSERT INTO Produto (Nome, Descricao, Custo, IdadeMinima, ImagemAlusiva, ManualAssociado)
VALUES
('Blast', 'Criado para destruir, mantido para salvar.', 50, 15, '/Products/Blast/blast-capa.png', 1),
('Charlie', 'Preferes que toque bateria?', 100, 11, '/Products/Charlie/charlie-capa.png', 2),
('Tricky', 'Atenção... Lá vai a bola... Cesto!', 150, 11, '/Products/Tricky/tricky-capa.png', 3);

-- Tabela Peça
INSERT INTO Peca (Nome, ImagemAlusiva, Quantidade)
VALUES
('Peça 1', '/Products/Blast/Pecas/blast-peca-1.png', 10),
('Peça 2', '/Products/Blast/Pecas/blast-peca-2.png', 10),
('Peça 3', '/Products/Blast/Pecas/blast-peca-3.png', 10),
('Peça 4', '/Products/Blast/Pecas/blast-peca-4.png', 10),
('Peça 5', '/Products/Blast/Pecas/blast-peca-5.png', 10),
('Peça 6', '/Products/Blast/Pecas/blast-peca-6.png', 10),
('Peça 7', '/Products/Charlie/Pecas/charlie-peca-1.png', 10),
('Peça 8', '/Products/Charlie/Pecas/charlie-peca-2.png', 10),
('Peça 9', '/Products/Charlie/Pecas/charlie-peca-3.png', 10),
('Peça 10', '/Products/Charlie/Pecas/charlie-peca-4.png', 10),
('Peça 11', '/Products/Charlie/Pecas/charlie-peca-5.png', 10),
('Peça 12', '/Products/Tricky/Pecas/tricky-peca-1.png', 10),
('Peça 13', '/Products/Tricky/Pecas/tricky-peca-2.png', 10);

-- Tabela Peça por Produto
INSERT INTO PecaProduto (IDPeca, IDProduto, Quantidade)
VALUES
(1, 1, 1),
(2, 1, 1),
(3, 1, 1),
(4, 1, 1),
(5, 1, 1),
(6, 1, 1),
(7, 2, 1),
(8, 2, 1),
(9, 2, 1),
(10, 2, 1),
(11, 2, 1),
(12, 3, 1),
(13, 3, 1);
GO
