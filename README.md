# Luckyu.V1

## 介绍
.Net Core 3.1 后台管理框架 <br/>
企业内部权限管理系统基本框架 <br/>
支持多公司 多部门 多岗位 划分,数据权限灵活配置, 项目分模块开发 <br/>
支持动态工作流设计, 工作流分版本 <br/>
支持动态数据权限配置 <br/>
![菜单搜索与换肤](https://images.gitee.com/uploads/images/2020/1029/171631_29728759_543243.gif "theme.gif")

![列表标题栏搜索](https://images.gitee.com/uploads/images/2020/1029/163810_55ecdce6_543243.gif "search2.gif")

![流程图设计](https://images.gitee.com/uploads/images/2020/1029/170356_f351c4ef_543243.jpeg "workflow.jpg")

## 说明
基于 Layui 后台框架, 整合了许多第三方插件如 jqgrid eletree xmselect等
仅作为学习尝试, 如有侵权, 可联系删除

数据库 MySql  日志库与业务主库分离, 日志库自动分表
ORM框架 FreeSql
前台框架 Layui 
缓存 Redis 与 MemoryCache 可切换

## 分模块开发
采取分项目,分模块开发
Luckyu.Web为主启动项目
各功能模块以 Luckyu.Module 开头
![输入图片说明](https://images.gitee.com/uploads/images/2020/1010/110915_53b08b55_543243.jpeg "1.jpg")

★★★重点★★★
各项目模块 新建项目类型为 RCL (Razor Class Library)
需要在.csproj文件中做如下配置

指定 dll 输出路径
```
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
<OutputPath>..\Luckyu.Web\bin\$(Configuration)\</OutputPath>
</PropertyGroup>
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
<OutputPath>..\Luckyu.Web\bin\$(Configuration)\</OutputPath>
</PropertyGroup>
```

复制 Areas 文件夹下静态文件
```
<ItemGroup>
<None Include="Areas\**\*.js">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
<None Include="Areas\**\*.css">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
</ItemGroup>

```

☆☆☆有关发布的说明☆☆☆
发布时请务必设置 VS 编译模式为 Release , 发布 Web 项目, 并且在 Release 模式下生成所有Module ,复制web/bin/release目录下所有dll覆盖发布文件dll,代码中目前有需要方便调试的判断都是根据 vs debug  release 模式写的,后期考虑做成动态配置文件

## 已有模块
### 组织机构 <br/>
公司管理<br/>
部门管理<br/>
小组管理<br/>
角色管理<br/>
岗位管理<br/>
用户管理<br/>
菜单管理<br/>
用户权限管理<br/>

### 系统设置  <br/>
数据字典分类 <br/>
数据字典编辑-管理员 <br/>
数据字典编辑-用户 <br/>
编码规则生成 <br/>

### 流程管理 <br/>
流程设计 <br/>
我的任务 - 发起的任务 待办任务 已办任务<br/>
流程监控 - 强制结束运行中的流程,调整当前待办任务到任意节点位置,并且可以切换流程版本<br/>
任务委托 - 指定时间段内指定模块审批任务交给指定人待批, 用于出差 请假时<br/>

### 办公管理 <br/>
通知管理 <br/>
请假管理 - 目前用于测试流程<br/>

## 下一步任务
安排有变,增加表管理 , 列管理统一设置验证,统一保存方法 <br/>
★模拟自动完成审批 <br/>
★审批流程配置增加是否可修改, 审批页面可以直接修改单据 <br/>
加入 singalir 做通知任务推送 <br/>
整理代码
