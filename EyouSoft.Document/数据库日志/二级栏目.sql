--2012-12-20
SET IDENTITY_INSERT [tbl_SysPrivs2] ON

--计调中心
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 1,1,'确认件登记','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 2,1,'地接安排','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 3,1,'票务安排','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 4,1,'回访提醒','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 5,1,'团队质量反馈','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 6,1,'线路管理','',0,'1')

--供应商管理
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 7,2,'地接','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 8,2,'票务','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 9,2,'酒店','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 10,2,'餐馆','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 11,2,'景点','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 12,2,'导游','',0,'1')

--财务管理
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 13,3,'应收管理','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 14,3,'应付管理','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 15,3,'其他收入','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 16,3,'其他支出','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 17,3,'出纳登账','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 18,3,'银行余额','',0,'1')

--统计中心
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 19,4,'团散统计','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 20,4,'组团社统计','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 21,4,'销售地区统计','',0,'1')

--客户管理
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 22,5,'客户关怀','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 23,5,'客户资料','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 24,5,'生日中心','',0,'1')

--客户询价
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 25,6,'客户日常询价','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 26,6,'外联每天足迹','',0,'1')

--公司文件
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 27,7,'公告通知','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 28,7,'文档管理','',0,'1')

--系统设置
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 29,8,'基础设置','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 30,8,'组织机构','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 31,8,'角色管理','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 32,8,'公司信息','',0,'1')
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 33,8,'系统日志','',0,'1')

--短信中心
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 34,9,'短信中心','',0,'1')

--计调中心-团队报价资料库
INSERT [tbl_SysPrivs2] ([Id],[ParentId],[Name],[Url],[SortId],[IsEnable]) VALUES ( 35,1,'团队报价资料库','',0,'1')


SET IDENTITY_INSERT [tbl_SysPrivs2] OFF


--生成二级栏目枚举SQL语句（将查询结果集另存为.txt文件即可）：
SELECT '/// <summary>
/// '+Menu1Name+'_'+Menu2Name+'
/// </summary>
'+Menu1Name+'_'+Menu2Name+' = '+CAST(Menu2Id AS VARCHAR(50))+','
FROM(SELECT A.Id AS Menu2Id,A.Name AS Menu2Name,B.Name AS Menu1Name FROM tbl_SysPrivs2 AS A INNER JOIN tbl_SysPrivs1 AS B ON A.ParentId=B.Id)C
