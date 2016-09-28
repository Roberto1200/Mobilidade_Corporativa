/*

CREATE TABLE lembretes
(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	mensagem VARCHAR(250) NOT NULL,
	data DATETIME NOT NULL,
	excluido BIT NOT NULL,
	id_administrador INT NOT NULL
)

CREATE TABLE lembretes_agentes
(
	id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	id_lembrete INT NOT NULL,
	id_usuario INT NOT NULL,
	data DATETIME NOT NULL
)
GO

INSERT INTO lembretes(mensagem, data, excluido, id_administrador)
VALUES('Mensagem de tamanho pequeno', GETDATE(), 0, 1)
GO
INSERT INTO lembretes(mensagem, data, excluido, id_administrador)
VALUES('Mensagem de tamanho pequeno', GETDATE(), 0, 2)
GO
INSERT INTO lembretes(mensagem, data, excluido, id_administrador)
VALUES('Mensagem de tamanho pequeno', GETDATE(), 0, 4)
GO
INSERT INTO lembretes(mensagem, data, excluido, id_administrador)
VALUES('Mensagem de tamanho pequeno', GETDATE(), 0, 22)
GO


INSERT INTO lembretes(mensagem, idRegional, data, excluido, idAdministradores)
VALUES('Mensagem de tamanho grande, para executar os testes de GRID, apenas para textos de grande proporção.', 63, GETDATE(), 0, 4)
GO
INSERT INTO lembretes(mensagem, idRegional, data, excluido, idAdministradores)
VALUES('Mensagem de tamanho grande, para executar os testes de GRID, apenas para textos de grande proporção.', 74, GETDATE(), 0, 4)
GO
INSERT INTO lembretes(mensagem, idRegional, data, excluido, idAdministradores)
VALUES('Mensagem de tamanho grande, para executar os testes de GRID, apenas para textos de grande proporção.', 78, GETDATE(), 0, 4)
GO
INSERT INTO lembretes(mensagem, idRegional, data, excluido, idAdministradores)
VALUES('Mensagem de tamanho grande, para executar os testes de GRID, apenas para textos de grande proporção.', 112, GETDATE(), 0, 4)
GO

*//*
ALTER PROCEDURE [dbo].[PC_SEL_LEMBRETES]
	@ID INT = NULL,
	@regional INT = NULL,
	@dataInicio DATETIME = NULL,
	@dataFim DATETIME = NULL,
	@procura VARCHAR(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		LEM.id,
		LEM.data,
		REG.id AS idRegional,
		REG.regional,
		LEM.mensagem
	FROM
		lembretes LEM
	INNER JOIN usuarios USR ON USR.id = LEM.id_administrador
	LEFT JOIN regionais REG ON REG.id = USR.idregional
	WHERE
		LEM.id = (CASE WHEN @ID IS NULL THEN LEM.ID ELSE @ID END)
	AND REG.id = (CASE WHEN @regional IS NULL THEN REG.id ELSE @regional END)
	AND LEM.data 
		BETWEEN 
			(CASE WHEN @dataInicio IS NULL THEN LEM.data ELSE @dataInicio END)
		AND 
			(CASE WHEN @dataFim IS NULL THEN LEM.data ELSE @dataFim END)
	AND LEM.mensagem LIKE (CASE WHEN @procura IS NULL THEN LEM.mensagem ELSE ('%' + @procura + '%') END)
	AND LEM.excluido = 0
END


SELECT * FROM usuarios ORDER BY juncao


GO

CREATE PROCEDURE [dbo].[PC_EXCLUIR_LEMBRETE]
	@ID INT
AS
BEGIN

    UPDATE lembretes SET excluido = 1 WHERE id = @ID

	RETURN @@ROWCOUNT

END

GO
*/

ALTER PROCEDURE [dbo].[PC_IU_LEMBRETES]
	@id_administrador INT,
	@msg VARCHAR(250),
	@AGENTES VARCHAR(250)
AS
BEGIN

	/****** Separando os IDs de Agentes *******/
	DECLARE @DELIMITADOR VARCHAR(100), @S VARCHAR(8000), @IDLEMBRETE INT
	SELECT @DELIMITADOR = ','
 
	IF LEN(@AGENTES) > 0 SET @AGENTES = @AGENTES + @DELIMITADOR 
	CREATE TABLE #AGENTES(ITEM_ARRAY VARCHAR(8000))
 
	WHILE LEN(@AGENTES) > 0
	BEGIN
	   SELECT @S = LTRIM(SUBSTRING(@AGENTES, 1, CHARINDEX(@DELIMITADOR, @AGENTES) - 1))
	   INSERT INTO #AGENTES (ITEM_ARRAY) VALUES (@S)
	   SELECT @AGENTES = SUBSTRING(@AGENTES, CHARINDEX(@DELIMITADOR, @AGENTES) + 1, LEN(@AGENTES))
	END
	/****** Separando os IDs de Agentes *******/
	INSERT INTO lembretes(mensagem, data, excluido, id_administrador)
	VALUES(@msg, GETDATE(), 0, @id_administrador);
	
	SET @IDLEMBRETE = SCOPE_IDENTITY();

	INSERT INTO lembretes_agentes(id_lembrete,id_usuario,data)
	SELECT @IDLEMBRETE, ITEM_ARRAY, GETDATE() FROM #AGENTES;
	
	DROP TABLE #AGENTES

END

SELECT * FROM lembretes_agentes

/*
select
	u.id, 
	u.nome 
from
	usuarios u
where
	u.diretoria = 4641 
and u.tipo = 1 -- Agentes
union
select 
	u.id, 
	u.nome 
from 
		   usuarios u
inner join usuarios_diretorias ud on u.id = ud.idusuario
where 
	ud.iddiretoria = 4641
and u.tipo = 2  -- Coordenadores
*/