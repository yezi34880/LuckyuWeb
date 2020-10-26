# Luckyu.V1

## 介绍
.Net Core 3.1 后台管理框架
![主页 换肤](https://images.gitee.com/uploads/images/2020/1010/113254_d1f46c89_543243.gif "b.gif")
![标题栏搜索](https://images.gitee.com/uploads/images/2020/1010/114008_76676dd0_543243.gif "c.gif")

## 说明
基于 Layui 后台框架, 整合了许多第三方插件如 jqgrid eletree xmselect等
仅作为学习尝试, 如有侵权, 可联系删除

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
 **组织机构** 
公司管理
部门管理
小组管理
角色管理
岗位管理
用户管理
菜单管理
用户权限管理

 **系统设置** 
数据字典分类
数据字典编辑-管理员
数据字典编辑-用户

 **流程管理** 
流程设计
我的任务
流程监控 - 强制结束运行中的流程

 **办公管理** 
通知管理 
请假管理 - 目前用于测试流程

 **下一步任务** 
增加任务委托,委托指定时间段内流程审批任务给某人,用于请假出差时
