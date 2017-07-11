USE [TeacherEvaluation]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[sp_studentLogin]
@username as char(10),
@password as char(32)
as
begin
select sID from student where sID=@username and password=@password
end
GO


CREATE proc [dbo].[sp_teacherLogin]
@username as char(10),
@password as char(32)
as 
begin
select tID from teacher where tID=@username and password=@password
end
GO


CREATE proc [dbo].[sp_managerLogin]
@username as char(10),
@password as char(32)
as 
begin
select mID from manager where mID=@username and password=@password
end
GO


CREATE procedure [dbo].[sp_getLatestTerm]
as
begin 
select top(1) term from teachingCourse order by term desc
end
GO


create procedure [dbo].[sp_getTcID]
@sID as char(10),
@tID as char(10),
@term as char(6)
as
begin
select tcID from teachingCourse
where tID=@tID 
and term=@term
and tcID in(
	select tcID from selectCourse
	where sID=@sID
)
end
GO


CREATE procedure [dbo].[sp_getTeachersBysID] 
@sID as char(10),
@term as char(6)
as
begin
select * from teacher
where tID in (
	select tID from teachingCourse
	where tcID in
	(
		select tcID from selectCourse
		where sID=@sID and term=@term
	)
)
end
GO


CREATE procedure [dbo].[sp_isGraded]
@sID as char(10),
@tcID as uniqueidentifier
as 
begin
select * from mark
where sID=@sID and tcID=@tcID
end
GO


CREATE procedure [dbo].[sp_submitMark]
@sID as char(10),
@tcID as uniqueidentifier,
@totalMark as tinyint,
@mark1 as tinyint,
@mark2 as tinyint,
@mark3 as tinyint,
@mark4 as tinyint,
@mark5 as tinyint
as
begin
insert into mark values(NEWID(),@tcID,@sID,@totalMark,@mark1,@mark2,@mark3,@mark4,@mark5)

update teacher set 
mark=(mark*numOfMarks+@totalMark)/(numOfMarks+1),
numOfMarks=numOfMarks+1
where tID in (
	select tID from teachingCourse
	where tcID=@tcID
)

update teachingCourse set mark=(mark*numOfMarks+@totalMark)/(numOfMarks+1),
numOfMarks=numOfMarks+1
where tcID=@tcID
end
GO


CREATE procedure [dbo].[sp_deleteMark]
@markID as uniqueidentifier
as begin
declare @tcID as uniqueidentifier
declare @totalMark as tinyint
declare @tID as char(10)
declare @tcNumOfMarks as int
declare @tNumOfMarks as int
set @tcID=(select tcID from mark where markID=@markID)
set @totalMark=(select totalMark from mark where markID=@markID)
set @tID=(select tID from teachingCourse where tcID=@tcID)
set @tcNumOfMarks=(select numOfMarks from teachingCourse where tcID=@tcID)
set @tNumOfMarks=(select numOfMarks from teacher where tID=@tID)

if @tcNumOfMarks=1
begin
update teachingCourse set mark=0,numOfMarks=0 where tcID=@tcID
end
else
begin
update teachingCourse set
mark=(mark*numOfMarks-@totalMark)/(numOfMarks-1),
numOfMarks=numOfMarks-1
where tcID=@tcID
end

if @tNumOfMarks=1
begin
update teacher set mark=0,numOfMarks=0 where tID=@tID
end
else
begin
update teacher set 
mark=(mark*numOfMarks-@totalMark)/(numOfMarks-1),
numOfMarks=numOfMarks-1
where tID=@tID
end

delete from mark where markID=@markID
end
GO