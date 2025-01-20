-- Tabela Administrador
CREATE TABLE Administrador (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(45),
    Email VARCHAR(45),
    PalavraPasse VARCHAR(250),
    Validado BIT DEFAULT 0
);

-- Tabela Instituição
CREATE TABLE Instituicao (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(45),
    NIF VARCHAR(25),
    NumeroAssociacao INT,
    Email VARCHAR(45),
    Morada VARCHAR(45),
    Creditos INT,
    PalavraPasse VARCHAR(250)
);

-- Tabela Colaborador
CREATE TABLE Colaborador (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(45),
    Email VARCHAR(45),
    DataNascimento DATE,
    InstituicaoID INT,
    PalavraPasse VARCHAR(250),
    FOREIGN KEY (InstituicaoID) REFERENCES Instituicao(ID)
);

-- Tabela Manual
CREATE TABLE Manual (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Capa VARCHAR(150),
    Nome VARCHAR(25),
    Descricao VARCHAR(75)
);

-- Tabela Manual da Instituição
CREATE TABLE ManualInstituicao (
    IDInstituicao INT,
    IDManual INT,
    PRIMARY KEY (IDInstituicao, IDManual),
    FOREIGN KEY (IDInstituicao) REFERENCES Instituicao(ID),
    FOREIGN KEY (IDManual) REFERENCES Manual(ID)
);

-- Tabela Página
CREATE TABLE Pagina (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ImagemAlusiva VARCHAR(150),
    Numeracao INT,
    ManualAssociado INT,
    FOREIGN KEY (ManualAssociado) REFERENCES Manual(ID)
);

-- Tabela Produto
CREATE TABLE Produto (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(25),
    Descricao VARCHAR(75),
    Custo INT,
    IdadeMinima INT,
    ImagemAlusiva VARCHAR(150),
    ManualAssociado INT,
    FOREIGN KEY (ManualAssociado) REFERENCES Manual(ID)
);

-- Tabela Peça
CREATE TABLE Peca (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(25),
    ImagemAlusiva VARCHAR(150),
    Quantidade INT
);

-- Tabela Peça por Produto
CREATE TABLE PecaProduto (
    IDPeca INT,
    IDProduto INT,
    Quantidade INT,
    PRIMARY KEY (IDPeca, IDProduto),
    FOREIGN KEY (IDPeca) REFERENCES Peca(ID),
    FOREIGN KEY (IDProduto) REFERENCES Produto(ID)
);

-- Tabela Compra
CREATE TABLE Compra (
    NumeroCompra INT IDENTITY(1,1) PRIMARY KEY,
    DataCompra DATE,
    EnderecoEntrega VARCHAR(45),
    ProdutoAssociado INT,
    InstituicaoID INT,
    FOREIGN KEY (ProdutoAssociado) REFERENCES Produto(ID),
    FOREIGN KEY (InstituicaoID) REFERENCES Instituicao(ID)
);
GO
