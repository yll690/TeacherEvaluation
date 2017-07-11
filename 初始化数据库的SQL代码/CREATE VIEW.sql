USE [TeacherEvaluation]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON


CREATE VIEW [dbo].[TCView]
AS
SELECT   dbo.course.cName, dbo.teacher.tName, dbo.teacher.tID, dbo.teachingCourse.tcID, dbo.teacher.profile, 
                dbo.teacher.picture, dbo.course.cID, dbo.teachingCourse.term, dbo.teachingCourse.numOfMarks AS tcNumOfMarks, 
                dbo.teacher.numOfMarks, dbo.teachingCourse.mark AS tcMark, dbo.teacher.mark, dbo.teacher.iID
FROM      dbo.course INNER JOIN
                dbo.teachingCourse ON dbo.course.cID = dbo.teachingCourse.cID INNER JOIN
                dbo.teacher ON dbo.teachingCourse.tID = dbo.teacher.tID


CREATE VIEW [dbo].[rankView]
AS
SELECT row_number() over(order by mark desc) as 全校排名
      ,(select rankNum from (select row_number() over(order by mark desc) rankNum,tID from teacher where iID=TCView.iID) a where a.tID=TCView.tID) as 学院排名
      ,(select rankNum from (select row_number() over(order by mark desc) rankNum,tID from teachingCourse where cID=TCView.cID) a where a.tID=TCView.tID) as 课程排名
      ,[iID] as 学院编号
      ,[tName] as 教师姓名
      ,[tID] as 教师编号
      ,[mark] as 教师得分
      ,[numOfMarks] as 评教人数
      ,[cName] as 课程名称
      ,[cID] as 课程编号
      ,[tcMark] as 授课得分
      ,[tcNumOfMarks] as 评课人数
      ,[term] as 学期
      ,[tcID] as 授课编号
  FROM [TeacherEvaluation].[dbo].[TCView]


CREATE VIEW [dbo].[commentView]
AS
SELECT   dbo.comment.comID, dbo.student.sName, dbo.student.picture, dbo.student.hideFromTeacher, 
                dbo.student.hideFromStudent, dbo.comment.cContent, dbo.comment.time, dbo.comment.tcID, dbo.comment.tReply
FROM      dbo.comment INNER JOIN
                dbo.student ON dbo.comment.sID = dbo.student.sID
WHERE   (dbo.comment.isHided = 0)


CREATE VIEW [dbo].[markView]
AS
SELECT   dbo.teacher.iID AS 学院编号, dbo.teacher.tName AS 教师姓名, dbo.teacher.tID AS 教师编号, 
                dbo.course.cName AS 课程名, dbo.course.cID AS 课程编号, dbo.teachingCourse.term AS 学期, 
                dbo.mark.totalMark AS 总体得分, dbo.mark.mark1 AS 教学质量, dbo.mark.mark2 AS 教学态度, 
                dbo.mark.mark3 AS 课堂氛围, dbo.mark.mark4 AS 作业批改, dbo.mark.mark5 AS 课后答疑, 
                dbo.teachingCourse.tcID AS 授课编号
FROM      dbo.mark INNER JOIN
                dbo.teachingCourse ON dbo.mark.tcID = dbo.teachingCourse.tcID INNER JOIN
                dbo.teacher ON dbo.teachingCourse.tID = dbo.teacher.tID INNER JOIN
                dbo.course ON dbo.teachingCourse.cID = dbo.course.cID
GO