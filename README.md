# Luckyu.V1

## 介绍
.Net Core 3.1 后台管理框架    
企业内部权限管理系统基本框架    
支持多公司 多部门 多岗位 划分,数据权限灵活配置, 项目分模块开发    
支持动态工作流设计, 工作流分版本    
支持动态数据权限配置    

菜单搜索与换肤  
![菜单搜索与换肤](./mdimage/1.gif)

列表标题栏搜索  
![列表标题栏搜索](./mdimage/2.gif)

自定义数据权限分配  
![自定义数据权限分配](./mdimage/12.jpg)

数据字典  
![数据字典](./mdimage/11.jpg)

审批流程设计  
![流程图设计](./mdimage/3.jpeg)

审批操作（协办、待办）  
![审批操作](./mdimage/31.jpg)

完整日志系统  
![完整日志系统](./mdimage/6.jpg)

手机版页面  
![手机版页面1](./mdimage/21.jpg)
![手机版页面2](./mdimage/22.jpg)
![手机版页面3](./mdimage/23.jpg)

## 说明
基于 Layui 后台框架, 整合了许多第三方插件如 jqgrid eletree xmselect等
仅作为学习尝试, 如有侵权, 可联系删除

数据库 MySql  日志库与业务主库分离, 日志库自动分表
ORM FreeSql  
前台框架 Layui   
缓存 Redis 与 MemoryCache 可切换  
利用 SingnalIR 实时推送消息  

## 分模块开发
采取分项目,分模块开发
Luckyu.Web为主启动项目
各功能模块以 Luckyu.Module 开头
![输入图片说明](./mdimage/4.jpeg)

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
### 组织机构  
公司管理   
部门管理   
小组管理   
角色管理   
岗位管理   
用户管理   
菜单管理   
用户权限管理   

### 系统设置     
数据字典分类    
数据字典编辑-管理员    
数据字典编辑-用户    
编码规则生成  
系统日志  
通知消息 - 利用SingnalIR给指定用户推送通知 

### 流程管理    
流程设计    
我的任务 - 发起的任务 待办任务 已办任务   
流程监控 - 强制结束运行中的流程,调整当前待办任务到任意节点位置,并且可以切换流程版本,管理员模拟自动通过流程审批   
任务委托 - 指定时间段内指定模块审批任务交给指定人待批, 用于出差 请假时   

### 办公管理    
通知管理    
请假管理 - 目前用于测试流程   

### 下一步任务
表单自定义字段

### 再下一步
表单多模板不同字段展示

## 更新日志
### 2021-7-6
优化了多公司、多部门
优化流程

### 2021-7-1
审批时增加附件上传，手机版也同步加上了
流程监控，调整流程，增加删除协办、代办人员的功能，增加删除审批历史记录功能

### 2021-6-25
修复审批相关BUG
小优化，当Layer弹框最小化后，可以打开其他的，多页面共存

### 2021-6-22
审批时增加上传附件
弹框最小化，可以打开多个

### 2021-6-20
审批修改原本加签为代办，即为把当前自己的审批任务让渡给他人
审批增加协办功能，即为自己待办审批期间，可以选择协办人，审批流程中会插入协办人待办任务，但协办人不管同意与否，流程并不向下流转，而是协办人办结后，当前审批人可以看到协办人意见，根据意见作出审批决断，此时流程才会向下流转。
注：不管代办、协办，增加人员之后，当前审批人依旧可以无视增加人直接批准。
审批驳回，增加选项，可以选择驳回至起始，或是驳回至上一步
手机审批同步增加代办、协办、驳回上一步功能。

### 2021-6-7
修复任务委托的bug

### 2021-6-1
手机审批后台也写好了

### 2021-5-9 
手机审批页面做好了，稍后来写后台逻辑

### 2021-4-21 
通用打印页面增加导出Excel

### 2021-4-21 
增加通用打印页面，使用HTML页面打印，暂时没有好用的报表
手机页面完成登录页、主页、列表页，还差审批页和表单页了


