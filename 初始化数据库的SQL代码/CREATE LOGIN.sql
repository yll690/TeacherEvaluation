USE [master]
GO

CREATE LOGIN [TE_student] WITH PASSWORD='student', DEFAULT_DATABASE=[TeacherEvaluation], DEFAULT_LANGUAGE=[简体中文], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF

CREATE LOGIN [TE_teacher] WITH PASSWORD='manager', DEFAULT_DATABASE=[TeacherEvaluation], DEFAULT_LANGUAGE=[简体中文], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF

CREATE LOGIN [TE_manager] WITH PASSWORD='manager', DEFAULT_DATABASE=[TeacherEvaluation], DEFAULT_LANGUAGE=[简体中文], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO


USE [TeacherEvaluation]

CREATE USER [TE_student] FOR LOGIN [TE_student] WITH DEFAULT_SCHEMA=[dbo]
grant select,insert,delete,update,execute to [TE_student]

CREATE USER [TE_teacher] FOR LOGIN [TE_teacher] WITH DEFAULT_SCHEMA=[dbo]
grant select,update,execute to [TE_teacher]

CREATE USER [TE_manager] FOR LOGIN [TE_manager] WITH DEFAULT_SCHEMA=[dbo]
grant select,delete,update,execute to [TE_manager]
GO