

GO

--后台服务
INSERT [tbl_SysPluging] ([PluginID],[Enabled],[PluginCategory]) VALUES ( 'fa6cb875-261c-45e3-b28b-6cd147b79c2b','1','定时短信服务')
GO

INSERT [tbl_SysPlugingSetting] ([PluginID],[PluginSettingName],[PluginSettingValue]) VALUES ( 'fa6cb875-261c-45e3-b28b-6cd147b79c2b','ExecuteOnAll','true')
INSERT [tbl_SysPlugingSetting] ([PluginID],[PluginSettingName],[PluginSettingValue]) VALUES ( 'fa6cb875-261c-45e3-b28b-6cd147b79c2b','Interval','1200000000')
GO