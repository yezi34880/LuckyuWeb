/*
 Navicat Premium Data Transfer

 Source Server         : MySQL-本机-127.0.0.1
 Source Server Type    : MySQL
 Source Server Version : 50717
 Source Host           : localhost:3306
 Source Schema         : lucky_core

 Target Server Type    : MySQL
 Target Server Version : 50717
 File Encoding         : 65001

 Date: 26/01/2021 15:03:11
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for oa_leave
-- ----------------------------
DROP TABLE IF EXISTS `oa_leave`;
CREATE TABLE `oa_leave`  (
  `leave_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `begintime` datetime(0) NOT NULL,
  `endtime` datetime(0) NOT NULL,
  `spantime` decimal(18, 1) NOT NULL,
  `leavetype` int(255) NOT NULL COMMENT '配置 事假 病假 产假 丧假',
  `reason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `state` int(255) NOT NULL COMMENT '0 起草 1 生效 2 报批 3 驳回',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`leave_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of oa_leave
-- ----------------------------
INSERT INTO `oa_leave` VALUES ('1335227273896398848', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2020-12-07 08:00:00', '2020-12-07 17:00:00', 8.0, 1, '家中有事', 1, '', '1', '超级管理员', '2020-12-05 22:19:24', NULL, '', '', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1335243218379476992', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2020-12-07 08:00:00', '2020-12-08 17:00:00', 16.0, 2, '发烧了', 1, '', '1', '超级管理员', '2020-12-05 23:22:46', NULL, '', '', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1335244433901359104', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-07 08:00:00', '2020-12-11 17:00:00', 40.0, 6, '年假出去旅游', 1, '', '1306793709295243264', '测试用户', '2020-12-05 23:27:35', NULL, '', '', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1335254300582088704', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-07 00:06:00', '2020-12-09 00:06:00', 16.0, 6, '随便请两天', 1, '', '1306793709295243264', '测试用户', '2020-12-06 00:06:48', '2020-12-06 14:55:10', '1306793709295243264', '测试用户', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1336677441883738112', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-31 22:21:00', '2021-01-01 22:21:00', 8.0, 1, '测试', 2, '', '1306793709295243264', '测试用户', '2020-12-09 22:21:51', '2020-12-09 22:36:36', '1306793709295243264', '测试用户', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1336677515674128384', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-09 22:22:00', '2020-12-09 22:22:00', 15.0, 2, '测试2', 2, '', '1306793709295243264', '测试用户', '2020-12-09 22:22:09', '2020-12-09 22:36:50', '1306793709295243264', '测试用户', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1336686154325561344', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2020-12-09 00:00:00', '2020-12-09 00:00:00', 0.0, 0, '', 0, '', '1', '超级管理员', '2020-12-09 22:56:28', NULL, '', '', 1, '1', '超级管理员', '2020-12-09 22:56:33');
INSERT INTO `oa_leave` VALUES ('1336686560988499968', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2020-12-09 00:00:00', '2020-12-09 00:00:00', 0.0, 0, '', 0, '', '1', '超级管理员', '2020-12-09 22:58:05', NULL, '', '', 1, '1', '超级管理员', '2020-12-09 22:58:10');
INSERT INTO `oa_leave` VALUES ('1336686820523642880', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2020-12-09 00:00:00', '2020-12-09 00:00:00', 0.0, 0, '', 0, '', '1', '超级管理员', '2020-12-09 22:59:07', NULL, '', '', 1, '1', '超级管理员', '2020-12-09 22:59:13');
INSERT INTO `oa_leave` VALUES ('1337043968633999360', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-11 22:37:00', '2020-12-30 22:37:00', 16.0, 1, '再来', 0, '', '1306793709295243264', '测试用户', '2020-12-10 22:38:18', '2020-12-10 22:58:29', '1306793709295243264', '测试用户', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1337261936483635200', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-24 13:04:00', '2020-12-31 13:04:00', 16.0, 1, '11111', 0, '', '1306793709295243264', '测试用户', '2020-12-11 13:04:25', '2020-12-11 13:15:26', '1306793709295243264', '测试用户', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1337263227800784896', '1306793709295243264', '测试用户', '1298639734708506624', '1297427797396033536', '2020-12-01 13:09:00', '2020-12-31 13:09:00', 160.0, 4, '回家生孩子', 0, '', '1306793709295243264', '测试用户', '2020-12-11 13:09:33', NULL, '', '', 0, '', '', NULL);
INSERT INTO `oa_leave` VALUES ('1348260081048883200', '1', '超级管理员', '1298639734708506624', '1297427797396033536', '2021-01-10 21:26:00', '2021-01-10 21:26:00', 1.0, 1, '111', 0, '', '1', '超级管理员', '2021-01-10 21:27:07', '2021-01-10 21:44:16', '1', '超级管理员', 0, '', '', NULL);

-- ----------------------------
-- Table structure for oa_news
-- ----------------------------
DROP TABLE IF EXISTS `oa_news`;
CREATE TABLE `oa_news`  (
  `news_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `catetory` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `keywords` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `source` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `publishtime` datetime(0) NOT NULL,
  `contents` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `is_publish` int(2) NOT NULL,
  `state` int(2) NOT NULL,
  `sort` int(2) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`news_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of oa_news
-- ----------------------------
INSERT INTO `oa_news` VALUES ('1318832748131127296', '安徽出台57条举措 让群众办事像网购一样方便', '新闻通知', '', '金台资讯', '2020-10-21 16:31:00', '<p style=\"margin-top: 0px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">人民网合肥10月21日电(周坤) 21日上午，在安徽省政府召开的进一步优化营商环境更好服务市场主体工作方案新闻发布会上，安徽省人民政府副秘书长、办公厅主任张杰对安徽省近日制定出台的《进一步优化营商环境更好服务市场主体工作方案》进行解读。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">张杰称，近年来，安徽省持续深化“放管服”改革，极大改善了营商环境，但还存在一些薄弱环节，特别是受新冠肺炎疫情影响，企业的生产经营仍然面临不少困难。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">为此，安徽省既关注企业当前面临的困难，又着眼于打破制约企业长期健康发展的体制机制障碍，制定出台《进一步优化营商环境更好服务市场主体工作方案》(以下简称，《工作方案》)。</span></p><p style=\"margin-top: 26px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">发布会上，张杰解读《进一步优化营商环境更好服务市场主体工作方案》。周坤 摄</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">据了解，《工作方案》包括持续提升投资建设便利度、进一步简化企业生产经营审批和条件、优化外贸外资企业经营环境等6个方面，20项工作内容、57条改革举措。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">通过减环节、压时限、强服务，改变过去企业百姓办事时间久、排队长、手续多，让“四最”营商环境，成为安徽省政府服务群众、服务企业的一个“金刚钻”和“金招牌”。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">张杰介绍，该《工作方案》充分体现安徽创新特色。通过加快“皖事通办”平台建设，构建企业群众统一办事平台，持续优化完善 7×24小时不打烊“随时办”服务，着力实现“市场主体和群众找政府办事像网购一样方便”。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">在推进长三角一体化发展方面，安徽将加强与沪苏浙联动，共同创建长三角区域国家级智能网联汽车先导区，推动更多涉企事项“跨省通办”、长三角“一网通办”，助力长三角一体化高质量发展。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">《工作方案》中还明确提出，支持各地进一步拓宽“地摊经济”场所和时间，实施审慎包容监管；对开展“共享用工”企业，给予就业补助资金补贴等；加快创新型医疗器械审评审批并推进临床应用，12月底前，实现行政审批时限比承诺时限缩减20%。</span></p><p style=\"margin-top: 22px; margin-bottom: 0px; padding: 0px; line-height: 24px; color: rgb(51, 51, 51); text-align: justify; font-family: arial; white-space: normal; background-color: rgb(255, 255, 255);\"><span class=\"bjh-p\">张杰表示，下一步，安徽省将对标国际国内先进水平，加大创新探索力度，推出更多务实管用的改革举措，全面优化流程环节、全面服务惠企利民、全面增强法治水平，不断放大政策效应，持续将营商环境改革向纵深推进，加快打造市场化法治化国际化营商环境。</span></p><p><img src=\"/upload/image/20201021/6373889650335255406187585.jpg\" title=\"5a794938aca5a.jpg\" alt=\"5a794938aca5a.jpg\" width=\"536\" height=\"317\" style=\"width: 536px; height: 317px;\"/></p>', 1, 1, 0, '', '1', '超级管理员', '2020-10-21 16:33:25', '2020-10-21 17:03:16', '1', '超级管理员', 0, '', '', NULL);
INSERT INTO `oa_news` VALUES ('1318842079190847488', '爱我人民爱我军 习近平这样锻造坚如磐石的军民关系', '新闻通知', '', '央视网', '2020-10-14 17:17:00', '<p>&nbsp; &nbsp; 2020年10月20日，全国双拥模范城（县）命名暨双拥模范单位和个人表彰大会举行，习近平总书记亲切会见与会代表，向他们表示诚挚问候，向受到命名表彰的全国双拥模范城（县）、双拥模范单位和个人表示热烈祝贺。</p><p>&nbsp; &nbsp; 习近平总书记始终高度重视双拥工作，多次在重要会议、国内考察时亲力亲为指导推动。他强调拥军优属、拥政爱民是我党我军特有的政治优势，坚如磐石的军政军民关系是我们战胜一切艰难险阻、不断从胜利走向胜利的重要法宝。</p><p>&nbsp; &nbsp; “难得举城作一庆，爱我人民爱我军。”军爱民，民拥军，最伟大的力量是同心合力。央视网《联播+》为您梳理总书记讲话，一起感受“军地合力，军民同心”汇聚起的磅礴力量。</p><p class=\"title\" style=\"box-sizing: border-box; text-size-adjust: none; padding: 0px; margin-top: 0.48rem; margin-bottom: 0.6rem; list-style: none; letter-spacing: 2px; color: rgb(51, 51, 51); font-size: 0.36rem; line-height: 0.6rem; text-align: justify; width: 745.1px;\"><img src=\"/upload/image/20201021/6373889697771858202780849.png\" alt=\"\" width=\"500\" style=\"font-family: &quot;PingFang SC&quot;, &quot;PingFang TC&quot;, &quot;Microsoft YaHei&quot;, STHeiti, arial, helvetica, sans-serif; font-size: 0.36rem; text-align: center; text-indent: 2em; box-sizing: border-box; border: 0px; display: block; width: 745.1px; box-shadow: rgba(0, 0, 0, 0.2) 0.02rem 0.04rem 0.05rem;\"/><img src=\"/ueupload/image/20201022/6373898291425161326583507.jpeg\" title=\"20170802002.jpeg\" alt=\"20170802002.jpeg\"/></p><p class=\"photo_img_20190808\" style=\"box-sizing: border-box; text-size-adjust: none; padding: 0px; margin-top: 0.48rem; margin-bottom: 0.48rem; list-style: none; letter-spacing: 2px; color: rgb(51, 51, 51); font-size: 0.36rem; line-height: 0.6rem; text-align: center; text-indent: 2em; font-family: &quot;PingFang SC&quot;, &quot;PingFang TC&quot;, &quot;Microsoft YaHei&quot;, STHeiti, arial, helvetica, sans-serif; white-space: normal; background-color: rgb(255, 255, 255);\"><img src=\"/upload/image/20201021/6373889697822698385642401.png\" alt=\"\" width=\"500\" style=\"box-sizing: border-box; border: 0px; display: block; width: 745.1px; box-shadow: rgba(0, 0, 0, 0.2) 0.02rem 0.04rem 0.05rem;\"/></p><p><br/></p>', 1, 1, 99, '', '1', '超级管理员', '2020-10-21 17:10:29', '2020-10-22 17:01:57', '1', '超级管理员', 0, '', '', NULL);
INSERT INTO `oa_news` VALUES ('1352974899303550976', '上海1地升为中风险 全国现有6+67个高中风险地区', '新闻通知', '上海,中风险', '新京报', '2021-01-23 21:41:00', '<p><img src=\"/ueupload/image/20210123/6374703491943502427073932.jpeg\"/></p>', 1, 1, 0, '', '1', '超级管理员', '2021-01-23 21:42:08', NULL, '', '', 0, '', '', NULL);

-- ----------------------------
-- Table structure for sys_annexfile
-- ----------------------------
DROP TABLE IF EXISTS `sys_annexfile`;
CREATE TABLE `sys_annexfile`  (
  `annex_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `external_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `filename` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `basepath` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `filepath` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `filesize` int(11) NOT NULL,
  `fileextenssion` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `contexttype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `downloadcount` int(255) NOT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`annex_id`) USING BTREE,
  INDEX `index1`(`annex_id`, `external_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_annexfile
-- ----------------------------
INSERT INTO `sys_annexfile` VALUES ('1348264427807444992', '1348260081048883200', '140904xr8h00ssn0g0zgza.jpg', 'annexbasepath', 'Leave\\1\\20210110\\1348264427807444992.jpg', 177813, '.jpg', 'image/jpeg', '', 0, '2021-01-10 21:44:24', '1', '超级管理员');

-- ----------------------------
-- Table structure for sys_authorize
-- ----------------------------
DROP TABLE IF EXISTS `sys_authorize`;
CREATE TABLE `sys_authorize`  (
  `id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `objecttype` int(11) NOT NULL COMMENT '1-角色 2-用户',
  `object_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `itemtype` int(11) NOT NULL COMMENT '1-菜单 2-按钮',
  `item_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_authorize
-- ----------------------------
INSERT INTO `sys_authorize` VALUES ('1301171361816907776', 1, '1300803540188532736', 1, '1', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907777', 1, '1300803540188532736', 1, '2', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907778', 1, '1300803540188532736', 1, '3', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907779', 1, '1300803540188532736', 1, '6', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907780', 1, '1300803540188532736', 1, '4', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907781', 1, '1300803540188532736', 1, '5', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907782', 1, '1300803540188532736', 1, '11', '', '1', '超级管理员', '2020-09-02 22:53:22');
INSERT INTO `sys_authorize` VALUES ('1301171361816907783', 1, '1300803540188532736', 1, '1300809545546862592', '', '1', '超级管理员', '2020-09-02 22:53:22');

-- ----------------------------
-- Table structure for sys_coderule
-- ----------------------------
DROP TABLE IF EXISTS `sys_coderule`;
CREATE TABLE `sys_coderule`  (
  `rule_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `rulecode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `rulename` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `contentjson` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`rule_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_coderule
-- ----------------------------
INSERT INTO `sys_coderule` VALUES ('1326183111041617920', 'LeaveNo', '请假编码', '[{\"ItemId\":\"ce0ada0b-acf9-ddc3-c52c-70067b96db90\",\"Type\":\"date\",\"TypeName\":\"日期时间\",\"Format\":\"yyMM\",\"BeNumber\":1},{\"ItemId\":\"e37836b6-c4e0-4867-256b-17baa2ed901c\",\"Type\":\"user\",\"TypeName\":\"人员\",\"Format\":\"\",\"BeNumber\":0},{\"ItemId\":\"1b43d651-258b-8711-1b4a-5a440fcc16a8\",\"Type\":\"number\",\"TypeName\":\"流水号\",\"Format\":\"000\",\"BeNumber\":0}]', '', '1', '超级管理员', '2020-11-10 23:21:08', '2020-11-10 23:32:18', '1', '超级管理员', 0, '', NULL, '', 1);

-- ----------------------------
-- Table structure for sys_coderule_seed
-- ----------------------------
DROP TABLE IF EXISTS `sys_coderule_seed`;
CREATE TABLE `sys_coderule_seed`  (
  `seed_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `rule_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `seedprename` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '前缀',
  `seedvalue` int(255) NOT NULL COMMENT '流水号',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`seed_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_coderule_seed
-- ----------------------------

-- ----------------------------
-- Table structure for sys_company
-- ----------------------------
DROP TABLE IF EXISTS `sys_company`;
CREATE TABLE `sys_company`  (
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `parent_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `companycode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `fullname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `shortname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `legalperson` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `country` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `province` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `counties` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '县区',
  `address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `foundeddate` datetime(0) NULL DEFAULT NULL COMMENT '成立时间',
  `website` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '主页',
  `sort` int(255) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`company_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_company
-- ----------------------------
INSERT INTO `sys_company` VALUES ('1297427797396033536', '0', '001', '阿里巴巴股份有限公司', '阿里', '马云', 'yuyang@163.com', '1', '1', '1', '1', '1', '1', '2020-08-23 00:00:00', '1', 0, '', '1', '超级管理员', '2020-08-23 14:57:47', '2020-08-23 16:58:37', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_company` VALUES ('1297458150068326400', '1297427797396033536', '002', '天猫国际股份有限公司', '天猫', '', '', '', '', '', '', '', '', '2020-08-01 00:00:00', '', 0, '', '1', '超级管理员', '2020-08-23 16:58:23', '2020-09-12 15:24:29', '1', '超级管理员', 0, '', NULL, '', 1);

-- ----------------------------
-- Table structure for sys_config
-- ----------------------------
DROP TABLE IF EXISTS `sys_config`;
CREATE TABLE `sys_config`  (
  `config_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `configcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `configname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `configvalue` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`config_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_config
-- ----------------------------
INSERT INTO `sys_config` VALUES ('111', 'annexbasepath', '附件目录地址', 'D:/MyProject/OpenSource/LuckyuWeb/File/AnnexFile', '默认附件目录位置，每年可以使用新路径 配置编码为【annexbasepath_yyyy】', '', '', NULL, '2021-01-26 15:00:13', '1', '超级管理员-system', 0, NULL, '', '', 1);

-- ----------------------------
-- Table structure for sys_dataauthorize
-- ----------------------------
DROP TABLE IF EXISTS `sys_dataauthorize`;
CREATE TABLE `sys_dataauthorize`  (
  `auth_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `modulename` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `objectrange` int(255) NOT NULL COMMENT '0自定义 1同公司 2同部门 3同小组 9全部',
  `objecttype` int(255) NOT NULL COMMENT '0用户 1岗位 2角色',
  `object_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `objectname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `staterange` int(11) NOT NULL DEFAULT 0 COMMENT '0 仅生效 1 全部状态',
  `edittype` int(255) NOT NULL COMMENT '0 仅查看 1 可修改起草驳回 2修改全部',
  `is_enable` int(255) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`auth_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dataauthorize
-- ----------------------------
INSERT INTO `sys_dataauthorize` VALUES ('1314217046006960128', 'Leave', 9, 0, '1', '超级管理员', 0, 0, 1, '管理员', '1', '超级管理员', '2020-10-08 22:52:15', NULL, '', '', 0, '', '', NULL);

-- ----------------------------
-- Table structure for sys_dataauthorize_detail
-- ----------------------------
DROP TABLE IF EXISTS `sys_dataauthorize_detail`;
CREATE TABLE `sys_dataauthorize_detail`  (
  `detail_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `auth_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `objecttype` int(255) NOT NULL COMMENT '0用户 1公司 2部门',
  `object_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `objectname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`detail_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dataauthorize_detail
-- ----------------------------

-- ----------------------------
-- Table structure for sys_dataitem
-- ----------------------------
DROP TABLE IF EXISTS `sys_dataitem`;
CREATE TABLE `sys_dataitem`  (
  `dataitem_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `itemcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `itemname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `parent_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_system` int(255) NOT NULL,
  `sort` int(11) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`dataitem_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dataitem
-- ----------------------------
INSERT INTO `sys_dataitem` VALUES ('1', 'organization', '组织机构', '0', 1, 1, NULL, '2020-09-12 21:24:08', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_dataitem` VALUES ('1305153951435460608', 'flowtype', '流程类型', '2', 1, 1, '', '2020-09-13 22:38:45', '1', '超级管理员', '2020-09-15 16:49:26', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1308776944787132416', 'oa', '办公', '0', 1, 9, '', '2020-09-23 22:35:14', '1', '超级管理员', '2020-09-23 22:38:44', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1308777176283353088', 'leavetype', '请假类型', '1308776944787132416', 0, 1, '', '2020-09-23 22:36:09', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1308777574587043840', 'system', '系统', '0', 1, 5, '', '2020-09-23 22:37:44', '1', '超级管理员', '2020-09-23 22:38:34', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1308777743881736192', 'state', '审批状态', '2', 1, 4, '', '2020-09-23 22:38:25', '1', '超级管理员', '2020-09-25 16:49:30', '1306793709295243264', '测试用户', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1309414846479077376', 'stateshow', '审批状态-标签', '2', 1, 6, '', '2020-09-25 16:50:02', '1306793709295243264', '测试用户', NULL, '', '', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1314211643311394816', 'modulename', '数据权限-模块', '1', 1, 1, '', '2020-10-08 22:30:47', '1', '超级管理员', '2020-10-08 22:57:41', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1314218336153899008', 'objecttype', '数据权限-查看着类型', '1', 1, 2, '', '2020-10-08 22:57:23', '1', '超级管理员', '2020-10-08 22:57:46', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1314218512541159424', 'objectrange', '数据权限-查看范围', '1', 1, 3, '', '2020-10-08 22:58:05', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('1318451052059693056', 'newscatetory', '新闻通知类型', '1308776944787132416', 0, 4, '', '2020-10-20 15:16:41', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1);
INSERT INTO `sys_dataitem` VALUES ('2', 'workflow', '流程', '0', 1, 2, NULL, '2020-09-12 21:25:16', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_dataitem` VALUES ('3', 'dataauthorize', '数据权限', '0', 1, 3, NULL, '2020-09-12 21:25:50', NULL, NULL, NULL, NULL, NULL, 0, '1', '2020-09-12 21:26:49', '超级管理员', 1);

-- ----------------------------
-- Table structure for sys_dataitem_detail
-- ----------------------------
DROP TABLE IF EXISTS `sys_dataitem_detail`;
CREATE TABLE `sys_dataitem_detail`  (
  `detail_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `dataitem_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `itemcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `showname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `itemvalue` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `itemvalue2` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(11) NOT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  `is_system` int(255) NOT NULL DEFAULT 0,
  PRIMARY KEY (`detail_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dataitem_detail
-- ----------------------------
INSERT INTO `sys_dataitem_detail` VALUES ('1', '1305153951435460608', 'flowtype', '合同类', '合同类', '', '', 1, '2020-09-13 23:01:46', '1', '超级管理员', '2020-09-13 23:08:46', '1', '超级管理员', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1305791089210429440', '1305153951435460608', 'flowtype', '办公类', '办公类', '', '', 2, '2020-09-15 16:50:31', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778001705603072', '1308777743881736192', 'state', '草稿', '0', '', '', 1, '2020-09-23 22:39:26', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778049562611712', '1308777743881736192', 'state', '生效', '1', '', '', 2, '2020-09-23 22:39:38', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778097293791232', '1308777743881736192', 'state', '审批中', '2', '', '', 3, '2020-09-23 22:39:49', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778147075985408', '1308777743881736192', 'state', '驳回', '-1', '', '', 4, '2020-09-23 22:40:01', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778242265714688', '1308777176283353088', 'leavetype', '事假', '1', '', '', 1, '2020-09-23 22:40:24', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778278231871488', '1308777176283353088', 'leavetype', '病假', '2', '', '', 2, '2020-09-23 22:40:32', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778304848924672', '1308777176283353088', 'leavetype', '婚假', '3', '', '', 3, '2020-09-23 22:40:38', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778333307277312', '1308777176283353088', 'leavetype', '产假', '4', '', '', 4, '2020-09-23 22:40:45', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778367012704256', '1308777176283353088', 'leavetype', '丧假', '5', '', '', 5, '2020-09-23 22:40:53', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1308778399732469760', '1308777176283353088', 'leavetype', '年休假', '6', '', '', 6, '2020-09-23 22:41:01', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1309415306103492608', '1309414846479077376', 'stateshow', '<span class=\"label label-default\">草稿</span>', '0', '', '', 0, '2020-09-25 16:51:51', '1306793709295243264', '测试用户', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1309415408759083008', '1309414846479077376', 'stateshow', '<span class=\"label label-success\">生效</span>', '1', '', '', 1, '2020-09-25 16:52:16', '1306793709295243264', '测试用户', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1309415510210908160', '1309414846479077376', 'stateshow', '<span class=\"label label-primary\">审批中</span>', '2', '', '', 2, '2020-09-25 16:52:40', '1306793709295243264', '测试用户', '2020-09-28 09:28:48', '1306793709295243264', '测试用户', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1309415604297535488', '1309414846479077376', 'stateshow', '<span class=\"label label-danger\">驳回</span>', '-1', '', '', 3, '2020-09-25 16:53:02', '1306793709295243264', '测试用户', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314211763083939840', '1314211643311394816', 'modulename', '请假', 'Leave', '', '', 1, '2020-10-08 22:31:16', '1', '超级管理员', '2020-10-08 22:44:34', '1', '超级管理员', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314218599644270592', '1314218336153899008', 'objecttype', '用户', '0', '', '', 1, '2020-10-08 22:58:26', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314218652739964928', '1314218336153899008', 'objecttype', '岗位', '1', '', '', 2, '2020-10-08 22:58:38', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314218686076293120', '1314218336153899008', 'objecttype', '角色', '2', '', '', 3, '2020-10-08 22:58:46', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314221158832410624', '1314218512541159424', 'objectrange', '自定义', '0', '', '', 1, '2020-10-08 23:08:36', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314221220388016128', '1314218512541159424', 'objectrange', '同公司', '1', '', '', 2, '2020-10-08 23:08:51', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314221252579299328', '1314218512541159424', 'objectrange', '同部门', '2', '', '', 3, '2020-10-08 23:08:58', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1314221292244832256', '1314218512541159424', 'objectrange', '全部', '9', '', '', 9, '2020-10-08 23:09:08', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);
INSERT INTO `sys_dataitem_detail` VALUES ('1318452283981303808', '1318451052059693056', 'newscatetory', '新闻通知', '新闻通知', '', '', 1, '2020-10-20 15:21:35', '1', '超级管理员', NULL, '', '', 0, '', NULL, '', 1, 0);

-- ----------------------------
-- Table structure for sys_dbcolumn
-- ----------------------------
DROP TABLE IF EXISTS `sys_dbcolumn`;
CREATE TABLE `sys_dbcolumn`  (
  `column_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `table_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `dbcolumnname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `showcolumnname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `dbtype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `showtype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'user department company dataitem datasource',
  `showformat` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `layverify` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `laylength` int(11) NOT NULL DEFAULT 6,
  `is_hidden` int(2) NOT NULL DEFAULT 0,
  PRIMARY KEY (`column_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dbcolumn
-- ----------------------------
INSERT INTO `sys_dbcolumn` VALUES ('1', '1', 'user_id', '用户', '', 'user', '', NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('2', '1', 'department_id', '部门', '', 'department', NULL, NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('3', '1', 'company_id', '公司', '', 'company', NULL, NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('4', '1', 'begintime', '开始时间', '', 'datetime', NULL, NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('5', '1', 'endtime', '结束时间', '', 'datetime', NULL, NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('6', '1', 'spantime', '请假时长(小时)', '', 'number', NULL, NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('7', '1', 'leavetype', '类型', '', 'dataitem', '{\"itemcode\":\"leavetype\"}', NULL, 6, 0);
INSERT INTO `sys_dbcolumn` VALUES ('8', '1', 'reason', '事由', '', 'text', NULL, NULL, 12, 0);

-- ----------------------------
-- Table structure for sys_dbtable
-- ----------------------------
DROP TABLE IF EXISTS `sys_dbtable`;
CREATE TABLE `sys_dbtable`  (
  `table_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `dbname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `showname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`table_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dbtable
-- ----------------------------
INSERT INTO `sys_dbtable` VALUES ('1', 'oa_leave', '请假', '', '', '', '2020-11-21 21:12:14');

-- ----------------------------
-- Table structure for sys_department
-- ----------------------------
DROP TABLE IF EXISTS `sys_department`;
CREATE TABLE `sys_department`  (
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `parent_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `departmentcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `fullname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `shortname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `phone` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(255) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`department_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_department
-- ----------------------------
INSERT INTO `sys_department` VALUES ('1', '1297427797396033536', '0', '01', '技术部', '技术部', '', '', 0, '', '1', '超级管理员', '2020-08-26 23:09:52', '2020-08-26 23:13:58', '1', '超级管理员', 0, '', NULL, '', 1);
INSERT INTO `sys_department` VALUES ('1298639734708506624', '1297427797396033536', '1', '002', '技术二部', '技术二部', '', '', 0, '', '1', '超级管理员', '2020-08-26 23:13:35', '2020-08-27 22:31:23', '1', '超级管理员', 0, '', NULL, '', 1);

-- ----------------------------
-- Table structure for sys_group
-- ----------------------------
DROP TABLE IF EXISTS `sys_group`;
CREATE TABLE `sys_group`  (
  `group_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `groupcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `groupname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(255) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`group_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_group
-- ----------------------------
INSERT INTO `sys_group` VALUES ('1300441145863704576', 'cs', '测试小组', 1, '1', '1', '超级管理员', '2020-08-31 22:31:45', '2020-09-18 11:44:54', '1', '超级管理员', 0, '', '', NULL, 1);

-- ----------------------------
-- Table structure for sys_message
-- ----------------------------
DROP TABLE IF EXISTS `sys_message`;
CREATE TABLE `sys_message`  (
  `message_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `catetory` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `contents` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `to_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `to_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `send_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `send_username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sendtime` datetime(0) NOT NULL,
  `is_read` int(2) NOT NULL,
  `readtime` datetime(0) NOT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`message_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_message
-- ----------------------------
INSERT INTO `sys_message` VALUES ('1353527367741607936', '系统通知', '测试消息1', '1', '超级管理员-system', '1', '超级管理员', '2021-01-25 10:17:26', 0, '1900-01-01 00:00:00', '1', '超级管理员', '2021-01-25 10:17:26', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353527367749996544', '系统通知', '测试消息1', '1306793709295243264', '测试用户-ceshi', '1', '超级管理员', '2021-01-25 10:17:26', 0, '1900-01-01 00:00:00', '1', '超级管理员', '2021-01-25 10:17:26', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353527545219387392', '系统通知', '测试2', '1', '超级管理员-system', '1', '超级管理员', '2021-01-25 10:18:09', 0, '1900-01-01 00:00:00', '1', '超级管理员', '2021-01-25 10:18:09', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353527703734718464', '系统通知', '测试3', '1', '超级管理员-system', '1', '超级管理员', '2021-01-25 10:18:47', 0, '1900-01-01 00:00:00', '1', '超级管理员', '2021-01-25 10:18:47', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353527842004144128', '系统通知', '测试4', '1', '超级管理员-system', '1', '超级管理员', '2021-01-25 10:19:20', 0, '1900-01-01 00:00:00', '1', '超级管理员', '2021-01-25 10:19:20', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353528397124472832', '系统通知', '测试10', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:21:32', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:21:32', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353528397137055744', '系统通知', '测试10', '1306793709295243264', '测试用户-ceshi', '1', '超级管理员-system', '2021-01-25 10:21:32', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:21:32', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353528397137055745', '系统通知', '测试10', '1306793948387348480', '测试用户2-ceshi2', '1', '超级管理员-system', '2021-01-25 10:21:32', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:21:32', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353530209952993280', '系统通知', '测试消息100', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:28:44', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:28:44', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353530653576138752', '系统通知', '消息', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:30:30', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:30:30', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353530799395311616', '系统通知', '消息', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:31:05', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:31:05', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353530876759248896', '系统通知', '消息', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:31:23', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:31:23', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353531983233421312', '系统通知', '通知', '1', '超级管理员-system', '1', '超级管理员-system', '2021-01-25 10:35:47', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:35:47', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353531983237615616', '系统通知', '通知', '1306793709295243264', '测试用户-ceshi', '1', '超级管理员-system', '2021-01-25 10:35:47', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:35:47', NULL, '', '', 0, '', '', NULL);
INSERT INTO `sys_message` VALUES ('1353532182458667008', '系统通知', '测试通知', '1306793709295243264', '测试用户-ceshi', '1', '超级管理员-system', '2021-01-25 10:36:34', 0, '1900-01-01 00:00:00', '1', '超级管理员-system', '2021-01-25 10:36:34', NULL, '', '', 0, '', '', NULL);

-- ----------------------------
-- Table structure for sys_module
-- ----------------------------
DROP TABLE IF EXISTS `sys_module`;
CREATE TABLE `sys_module`  (
  `module_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `parent_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `modulecode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `modulename` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `moduleurl` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `moduleicon` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(11) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(11) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `is_enable` int(11) NOT NULL,
  PRIMARY KEY (`module_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_module
-- ----------------------------
INSERT INTO `sys_module` VALUES ('1', '0', 'organization', '组织机构', NULL, 'layui-icon layui-icon-group', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('10', '0', 'system', '系统设置', NULL, 'layui-icon layui-icon-set', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('11', '1', 'module', '菜单管理', '/OrganizationModule/Module/Index', '', 8, '', NULL, NULL, NULL, '2020-09-03 23:03:43', '1', '超级管理员', 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('12', '10', 'dataitem', '数据字典分类', '/SystemModule/Dataitem/ClassifyIndex', '', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('13', '10', 'dataitemdetail', '数据字段明细', '/SystemModule/Dataitem/Index', '', 4, '', NULL, NULL, NULL, '2020-09-12 23:26:41', '1', '超级管理员', 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('1300809545546862592', '1', 'dataauthorize', '数据权限管理', '/OrganizationModule/DataAuthorize/Index', '', 10, '', '1', '超级管理员', '2020-09-01 22:55:38', '2020-10-07 14:45:47', '1', '超级管理员', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1301536109125308416', '1', 'user', '用户管理', '/OrganizationModule/User/Index', '', 6, '', '1', '超级管理员', '2020-09-03 23:02:44', '2020-09-03 23:03:34', '1', '超级管理员', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1304803595438395392', '10', 'dataitemdetailsystem', '数据字典明细-管理员', '/SystemModule/Dataitem/SystemIndex', '', 3, '', '1', '超级管理员', '2020-09-12 23:26:34', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1305783733730807808', '0', 'workflow', '流程管理', '', 'layui-icon layui-icon-link', 3, '', '1', '超级管理员', '2020-09-15 16:21:17', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1305783880967655424', '1305783733730807808', 'designer', '流程设计', '/WorkflowModule/Designer/Index', '', 1, '', '1', '超级管理员', '2020-09-15 16:21:52', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1308753678907346944', '1305783733730807808', 'mytask', '我的任务', '/WorkflowModule/Task/Index', '', 5, '', '1', '超级管理员', '2020-09-23 21:02:47', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1308754116331311104', '0', 'oa', '办公管理', '', 'layui-icon layui-icon-form', 10, '', '1', '超级管理员', '2020-09-23 21:04:31', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1308754213349756928', '1308754116331311104', 'leave', '请假管理', '/OAModule/Leave/Index', '', 0, '', '1', '超级管理员', '2020-09-23 21:04:55', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1318445122542768128', '1308754116331311104', 'news', '新闻通知', '/OAModule/News/Index', '', 5, '', '1', '超级管理员', '2020-10-20 14:53:07', '2020-10-20 15:05:55', '1', '超级管理员', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1321834335111876608', '1305783733730807808', 'delegate', '任务委托', '/WorkflowModule/Delegate/Index', '', 8, '', '1', '超级管理员', '2020-10-29 23:20:39', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1323639339174989824', '10', 'coderule', '编码规则', '/SystemModule/CodeRule/Index', '', 10, '', '1', '超级管理员', '2020-11-03 22:53:05', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1333041917851734016', '10', 'log', '系统日志', '/SystemModule/Log/Index', '', 15, '', '1', '超级管理员', '2020-11-29 21:35:35', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('1353517145971101696', '10', 'message', '消息通知', '/SystemModule/Message/Index', '', 30, '', '1', '超级管理员', '2021-01-25 09:36:49', NULL, '', '', 0, '', '', NULL, 1);
INSERT INTO `sys_module` VALUES ('14', '10', 'config', '配置字段', '/SystemModule/Config/Index', '', 1, '', NULL, NULL, NULL, '2020-09-12 23:26:47', '1', '超级管理员', 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('15', '1305783733730807808', 'monitor', '流程监控', '/WorkflowModule/Monitor/Index', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('2', '1', 'company', '公司管理', '/OrganizationModule/Company/Index', '', 1, '', NULL, NULL, NULL, '2020-09-01 23:08:36', '1', '超级管理员', 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('3', '1', 'department', '部门管理', '/OrganizationModule/Department/Index', '', 2, '', NULL, NULL, NULL, '2020-09-01 23:08:50', '1', '超级管理员', 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('4', '1', 'post', '岗位管理', '/OrganizationModule/Post/Index', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('5', '1', 'role', '角色管理', '/OrganizationModule/Role/Index', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);
INSERT INTO `sys_module` VALUES ('6', '1', 'group', '小组管理', '/OrganizationModule/Group/Index', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1);

-- ----------------------------
-- Table structure for sys_post
-- ----------------------------
DROP TABLE IF EXISTS `sys_post`;
CREATE TABLE `sys_post`  (
  `post_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `postcode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `postname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(255) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`post_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_post
-- ----------------------------
INSERT INTO `sys_post` VALUES ('1299976757956448256', '001', '董事长', 0, '', '1', '超级管理员', '2020-08-30 15:46:26', '2020-08-30 16:00:50', '1', '超级管理员', 0, '', '', NULL, 1);
INSERT INTO `sys_post` VALUES ('1299976813929435136', '002', '总经理', 0, '', '1', '超级管理员', '2020-08-30 15:46:39', '2020-08-30 16:00:55', '1', '超级管理员', 0, '', '', NULL, 1);

-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role`  (
  `role_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `rolecode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `rolename` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sort` int(11) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(11) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(11) NOT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`role_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_role
-- ----------------------------
INSERT INTO `sys_role` VALUES ('1300803540188532736', '001', '测试角色', 1, '测试', NULL, NULL, NULL, '2020-09-01 22:37:54', '1', '超级管理员', 0, NULL, NULL, 1, NULL);

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user`  (
  `user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `usercode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `realname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nickname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `loginname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `loginpassword` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `loginsecret` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `sex` int(255) NOT NULL COMMENT '1男 2女',
  `mobile` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `birthday` datetime(0) NULL DEFAULT NULL,
  `qq` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `wechat` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `level` int(11) NOT NULL COMMENT '0-一般用户  1-管理员 2-超级管理员',
  `sort` int(11) NOT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(11) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(11) NOT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`user_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO `sys_user` VALUES ('1', '1297427797396033536', '1298639734708506624', 'system', '超级管理员', '超级管理员', 'system', 'abbd26a09cf9b18e41cd7b3aa2b90e4c', 'system', 0, NULL, NULL, NULL, NULL, NULL, 99, 0, NULL, NULL, NULL, '2020-08-15 16:49:25', NULL, NULL, NULL, 0, NULL, NULL, 1, NULL);
INSERT INTO `sys_user` VALUES ('1306793709295243264', '1297427797396033536', '1298639734708506624', '', '测试用户', '', 'ceshi', '888', '', 0, '', '', NULL, '', '', 99, 0, '', '1', '超级管理员', '2020-09-18 11:14:34', NULL, '', '', 0, '', '', 1, NULL);
INSERT INTO `sys_user` VALUES ('1306793948387348480', '1297427797396033536', '1298639734708506624', '', '测试用户2', '', 'ceshi2', '888', '', 0, '', '', NULL, '', '', 99, 0, '', '1', '超级管理员', '2020-09-18 11:15:31', NULL, '', '', 0, '', '', 1, NULL);

-- ----------------------------
-- Table structure for sys_userrelation
-- ----------------------------
DROP TABLE IF EXISTS `sys_userrelation`;
CREATE TABLE `sys_userrelation`  (
  `id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `relationtype` int(11) NOT NULL COMMENT '1-角色 2-岗位 3-用户组 4-部门主管',
  `object_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_userrelation
-- ----------------------------
INSERT INTO `sys_userrelation` VALUES ('1302619651561361408', '1', 4, '1', '', '1', '超级管理员', '2020-09-06 22:48:21');
INSERT INTO `sys_userrelation` VALUES ('1302619651561361409', '1', 4, '1298639734708506624', '', '1', '超级管理员', '2020-09-06 22:48:21');
INSERT INTO `sys_userrelation` VALUES ('1302619888023638016', '1', 2, '1299976813929435136', '', '1', '超级管理员', '2020-09-06 22:49:17');
INSERT INTO `sys_userrelation` VALUES ('1302626926044778496', '1', 1, '1300803540188532736', '', '1', '超级管理员', '2020-09-06 23:17:15');

-- ----------------------------
-- Table structure for wf_delegate
-- ----------------------------
DROP TABLE IF EXISTS `wf_delegate`;
CREATE TABLE `wf_delegate`  (
  `delegate_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `starttime` datetime(0) NOT NULL,
  `endtime` datetime(0) NOT NULL,
  `flowcode` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `to_user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `to_username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(11) NOT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(11) NOT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`delegate_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_delegate
-- ----------------------------

-- ----------------------------
-- Table structure for wf_flow
-- ----------------------------
DROP TABLE IF EXISTS `wf_flow`;
CREATE TABLE `wf_flow`  (
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flowcode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `flowname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `flowtype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `flowrange` int(255) NOT NULL DEFAULT 0 COMMENT '范围 0全部 1指定',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `edittime` datetime(0) NULL DEFAULT NULL,
  `edit_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `edit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_delete` int(255) NOT NULL,
  `deletetime` datetime(0) NULL DEFAULT NULL,
  `delete_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `delete_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_enable` int(255) NOT NULL,
  PRIMARY KEY (`flow_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_flow
-- ----------------------------
INSERT INTO `wf_flow` VALUES ('1306861995051585536', 'leave', '请假', '办公类', 0, '', '1', '超级管理员', '2020-09-18 15:45:55', '2020-12-06 14:55:01', '1', '超级管理员', 0, NULL, '', '', 1);

-- ----------------------------
-- Table structure for wf_flow_authorize
-- ----------------------------
DROP TABLE IF EXISTS `wf_flow_authorize`;
CREATE TABLE `wf_flow_authorize`  (
  `auth_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `objecttype` int(255) NOT NULL COMMENT '1 用户 2 部门 3 公司 4岗位 5 角色',
  `object_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`auth_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_flow_authorize
-- ----------------------------

-- ----------------------------
-- Table structure for wf_flow_instance
-- ----------------------------
DROP TABLE IF EXISTS `wf_flow_instance`;
CREATE TABLE `wf_flow_instance`  (
  `instance_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `process_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单据编码',
  `flowcode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `flowname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `processname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `processcontent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `submit_user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `submit_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_finished` int(255) NOT NULL,
  `schemejson` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_userId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`instance_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_flow_instance
-- ----------------------------
INSERT INTO `wf_flow_instance` VALUES ('1335227549910962176', '1306861995051585536', '1335227273896398848', 'leave', '请假', '请假申请 1335227273896398848', '{\"id\":\"1335227273896398848\",\"user_id\":\"1\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-07T08:00:00\",\"endtime\":\"2020-12-07T17:00:00\",\"spantime\":8.0,\"leavetype\":1,\"reason\":\"家中有事\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1\",\"create_username\":\"超级管理员\",\"createtime\":\"2020-12-05T22:19:24.0774059+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1', '超级管理员', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\", \"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"审批\",\"left\":361,\"top\":231,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = 3\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"38740d9d-bcca-695d-e6ee-242a35b35331\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"}]}', '', '1', '超级管理员', '2020-12-05 22:20:30', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1335243218836656128', '1306861995051585536', '1335243218379476992', 'leave', '请假', '请假申请 1335243218379476992', '{\"id\":\"1335243218379476992\",\"user_id\":\"1\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-07T08:00:00\",\"endtime\":\"2020-12-08T17:00:00\",\"spantime\":16.0,\"leavetype\":2,\"reason\":\"发烧了\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1\",\"create_username\":\"超级管理员\",\"createtime\":\"2020-12-05T23:22:45.5386435+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1', '超级管理员', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\", \"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"审批\",\"left\":361,\"top\":231,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = 3\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"38740d9d-bcca-695d-e6ee-242a35b35331\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"}]}', '', '1', '超级管理员', '2020-12-05 23:22:46', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1335244434073325568', '1306861995051585536', '1335244433901359104', 'leave', '请假', '请假申请 1335244433901359104', '{\"id\":\"1335244433901359104\",\"user_id\":\"1306793709295243264\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-07T08:00:00\",\"endtime\":\"2020-12-11T17:00:00\",\"spantime\":40.0,\"leavetype\":6,\"reason\":\"年假出去旅游\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1306793709295243264\",\"create_username\":\"测试用户\",\"createtime\":\"2020-12-05T23:27:35.3415326+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\", \"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"审批\",\"left\":361,\"top\":231,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = 3\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"38740d9d-bcca-695d-e6ee-242a35b35331\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"}]}', '', '1306793709295243264', '测试用户', '2020-12-05 23:27:35', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1335254301001519104', '1306861995051585536', '1335254300582088704', 'leave', '请假', '请假申请 1335254300582088704', '{\"id\":\"1335254300582088704\",\"user_id\":\"1306793709295243264\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-07T00:06:00\",\"endtime\":\"2020-12-09T00:06:00\",\"spantime\":16.0,\"leavetype\":6,\"reason\":\"随便请两天\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1306793709295243264\",\"create_username\":\"测试用户\",\"createtime\":\"2020-12-06T00:06:47.7414822+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = 3\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-06 00:06:48', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1335477865671692288', '1306861995051585536', '1335254300582088704', 'leave', '请假', '请假申请 1335254300582088704', '{\"id\":\"1335254300582088704\",\"user_id\":\"\",\"department_id\":\"\",\"company_id\":\"\",\"begintime\":\"2020-12-07T00:06:00\",\"endtime\":\"2020-12-09T00:06:00\",\"spantime\":16.0,\"leavetype\":6,\"reason\":\"随便请两天\",\"state\":0,\"remark\":\"\",\"create_userid\":null,\"create_username\":\"\",\"createtime\":null,\"edittime\":\"2020-12-06T14:55:09.7310478+08:00\",\"edit_userid\":\"1306793709295243264\",\"edit_username\":\"测试用户\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-06 14:55:10', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1336677442303168512', '1306861995051585536', '1336677441883738112', 'leave', '请假', '请假申请 1336677441883738112', '{\"id\":\"1336677441883738112\",\"user_id\":\"1306793709295243264\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-31T22:21:00\",\"endtime\":\"2021-01-01T22:21:00\",\"spantime\":8.0,\"leavetype\":1,\"reason\":\"测试\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1306793709295243264\",\"create_username\":\"测试用户\",\"createtime\":\"2020-12-09T22:21:51.0685064+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-09 22:21:51', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1336677515871260672', '1306861995051585536', '1336677515674128384', 'leave', '请假', '请假申请 1336677515674128384', '{\"id\":\"1336677515674128384\",\"user_id\":\"1306793709295243264\",\"department_id\":\"1298639734708506624\",\"company_id\":\"1297427797396033536\",\"begintime\":\"2020-12-09T22:22:00\",\"endtime\":\"2020-12-09T22:22:00\",\"spantime\":15.0,\"leavetype\":2,\"reason\":\"测试2\",\"state\":0,\"remark\":\"\",\"create_userid\":\"1306793709295243264\",\"create_username\":\"测试用户\",\"createtime\":\"2020-12-09T22:22:08.66125+08:00\",\"edittime\":null,\"edit_userid\":\"\",\"edit_username\":\"\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 1, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-09 22:22:09', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1336681152248942592', '1306861995051585536', '1336677441883738112', 'leave', '请假', '请假申请 1336677441883738112', '{\"id\":\"1336677441883738112\",\"user_id\":\"\",\"department_id\":\"\",\"company_id\":\"\",\"begintime\":\"2020-12-31T22:21:00\",\"endtime\":\"2021-01-01T22:21:00\",\"spantime\":8.0,\"leavetype\":1,\"reason\":\"测试\",\"state\":0,\"remark\":\"\",\"create_userid\":null,\"create_username\":\"\",\"createtime\":null,\"edittime\":\"2020-12-09T22:36:35.5582237+08:00\",\"edit_userid\":\"1306793709295243264\",\"edit_username\":\"测试用户\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 0, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-09 22:36:36', '1297427797396033536', '1298639734708506624');
INSERT INTO `wf_flow_instance` VALUES ('1336681211438960640', '1306861995051585536', '1336677515674128384', 'leave', '请假', '请假申请 1336677515674128384', '{\"id\":\"1336677515674128384\",\"user_id\":\"\",\"department_id\":\"\",\"company_id\":\"\",\"begintime\":\"2020-12-09T22:22:00\",\"endtime\":\"2020-12-09T22:22:00\",\"spantime\":15.0,\"leavetype\":2,\"reason\":\"测试2\",\"state\":0,\"remark\":\"\",\"create_userid\":null,\"create_username\":\"\",\"createtime\":null,\"edittime\":\"2020-12-09T22:36:49.7582564+08:00\",\"edit_userid\":\"1306793709295243264\",\"edit_username\":\"测试用户\",\"is_delete\":0,\"deletetime\":null,\"delete_userid\":\"\",\"delete_username\":\"\",\"ExtObject\":null,\"ExtDataJson\":\"null\"}', '1306793709295243264', '测试用户', 0, '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}', '', '1306793709295243264', '测试用户', '2020-12-09 22:36:50', '1297427797396033536', '1298639734708506624');

-- ----------------------------
-- Table structure for wf_flow_scheme
-- ----------------------------
DROP TABLE IF EXISTS `wf_flow_scheme`;
CREATE TABLE `wf_flow_scheme`  (
  `scheme_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `schemejson` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`scheme_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_flow_scheme
-- ----------------------------
INSERT INTO `wf_flow_scheme` VALUES ('1335477829084778496', '1306861995051585536', '{\"nodes\":[{\"id\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"name\":\"结束\",\"left\":415,\"top\":424,\"type\":\"endround\",\"width\":52,\"height\":52},{\"id\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"name\":\"开始\",\"left\":405,\"top\":34,\"type\":\"startround\",\"width\":52,\"height\":52,\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}],\"sqlsuccess\":\"update oa_leave set state = 2\\nwhere id = @processId\",\"sqlfail\":\"\",\"injectprogram\":\"\",\"sqlcondition\":\"\",\"authusers\":[]},{\"id\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"name\":\"超级管理员审批\",\"left\":365,\"top\":292,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\",\"sqlsuccess\":\"update oa_leave set state = 1\\nwhere id = @processId\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1\",\"objectnames\":\"超级管理员\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]},{\"id\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"name\":\"测试用户2审批\",\"left\":362,\"top\":157,\"type\":\"stepnode\",\"width\":150,\"height\":65,\"dbFailId\":\"\",\"dbFailSql\":\"\",\"auditors\":[],\"wfForms\":[],\"authorizeFields\":[],\"iocName\":\"\",\"dbSuccessId\":\"\",\"dbSuccessSql\":\"\",\"timeoutAction\":48,\"timeoutNotice\":24,\"confluence_type\":\"1\",\"confluence_rate\":\"\",\"sqlsuccess\":\"\",\"sqlfail\":\"update oa_leave set state = -1\\nwhere id = @processId\",\"injectassembly\":\"\",\"injectclass\":\"\",\"sqlcondition\":\"\",\"authusers\":[{\"objecttype\":\"1\",\"objectids\":\"1306793948387348480\",\"objectnames\":\"测试用户2\",\"objectrange\":\"\"}],\"forms\":[{\"formname\":\"请假\",\"formurl\":\"/OAModule/Leave/Form\"}]}],\"lines\":[{\"id\":\"d6e1d37e-2e29-12b9-591d-3ee8c378f3db\",\"from\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"0\",\"type\":\"sl\"},{\"id\":\"8df5698a-d26b-db29-5f70-f9fdf3c4dec1\",\"from\":\"e3b1dc31-cc41-2702-5de8-49bf8fd20e62\",\"to\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"2d682a0a-4bd4-d655-1f74-469ca20dfd5a\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"fcde8608-a060-3fb7-b066-71e74d577d2c\",\"sp\":\"bottom\",\"ep\":\"top\",\"name\":\"\",\"wftype\":\"1\",\"type\":\"sl\"},{\"id\":\"52f5406e-c0de-036e-8a36-c21cee20ddda\",\"from\":\"af55b2d7-1e63-2434-9611-4104893a1da0\",\"to\":\"444ae837-e699-ab72-f58d-31991db832e9\",\"sp\":\"right\",\"ep\":\"right\",\"name\":\"\",\"wftype\":\"2\",\"type\":\"lr\",\"M\":582.5}]}');

-- ----------------------------
-- Table structure for wf_task
-- ----------------------------
DROP TABLE IF EXISTS `wf_task`;
CREATE TABLE `wf_task`  (
  `task_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `instance_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `process_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `processname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `node_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nodename` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nodetype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `authorize_user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `authorizen_userame` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `previous_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `previousname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_done` int(255) NOT NULL COMMENT '1生成历史',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`task_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_task
-- ----------------------------
INSERT INTO `wf_task` VALUES ('1335244434534699009', '1306861995051585536', '1335244434073325568', '1335244433901359104', '请假申请 1335244433901359104', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', 'stepnode', '', '', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '2020-12-05 23:27:35', '1306793709295243264', '测试用户', 1, '');
INSERT INTO `wf_task` VALUES ('1335476747449274368', '1306861995051585536', '1335254301001519104', '1335254300582088704', '请假申请 1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', '', '', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '2020-12-06 14:50:43', '1', '超级管理员', 1, '');
INSERT INTO `wf_task` VALUES ('1335478030402981888', '1306861995051585536', '1335477865671692288', '1335254300582088704', '请假申请 1335254300582088704', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', 'stepnode', '', '', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '2020-12-06 14:55:49', '1', '超级管理员', 1, '');
INSERT INTO `wf_task` VALUES ('1336677443397881856', '1306861995051585536', '1336677442303168512', '1336677441883738112', '请假申请 1336677441883738112', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', '', '', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '2020-12-09 22:21:51', '1306793709295243264', '测试用户', 0, '');
INSERT INTO `wf_task` VALUES ('1336677516676567041', '1306861995051585536', '1336677515871260672', '1336677515674128384', '请假申请 1336677515674128384', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', '', '', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '2020-12-09 22:22:09', '1306793709295243264', '测试用户', 0, '');
INSERT INTO `wf_task` VALUES ('1336681153322684416', '1306861995051585536', '1336681152248942592', '1336677441883738112', '请假申请 1336677441883738112', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', '', '', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '2020-12-09 22:36:36', '1306793709295243264', '测试用户', 0, '');
INSERT INTO `wf_task` VALUES ('1336681212277821441', '1306861995051585536', '1336681211438960640', '1336677515674128384', '请假申请 1336677515674128384', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', '', '', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '2020-12-09 22:36:50', '1306793709295243264', '测试用户', 0, '');

-- ----------------------------
-- Table structure for wf_task_authorize
-- ----------------------------
DROP TABLE IF EXISTS `wf_task_authorize`;
CREATE TABLE `wf_task_authorize`  (
  `auth_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `task_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `department_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `company_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `role_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `post_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `group_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `manage_dept_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `is_add` int(255) NOT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`auth_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_task_authorize
-- ----------------------------
INSERT INTO `wf_task_authorize` VALUES ('1335244434534699010', '1335244434534699009', '1', '', '', '', '', '', '', 0, '1306793709295243264');
INSERT INTO `wf_task_authorize` VALUES ('1335476747449274369', '1335476747449274368', '1306793948387348480', '', '', '', '', '', '', 0, '1');
INSERT INTO `wf_task_authorize` VALUES ('1335478030402981889', '1335478030402981888', '1', '', '', '', '', '', '', 0, '1');
INSERT INTO `wf_task_authorize` VALUES ('1336677443402076160', '1336677443397881856', '1306793948387348480', '', '', '', '', '', '', 0, '1306793709295243264');
INSERT INTO `wf_task_authorize` VALUES ('1336677516676567042', '1336677516676567041', '1306793948387348480', '', '', '', '', '', '', 0, '1306793709295243264');
INSERT INTO `wf_task_authorize` VALUES ('1336681153326878720', '1336681153322684416', '1306793948387348480', '', '', '', '', '', '', 0, '1306793709295243264');
INSERT INTO `wf_task_authorize` VALUES ('1336681212277821442', '1336681212277821441', '1306793948387348480', '', '', '', '', '', '', 0, '1306793709295243264');

-- ----------------------------
-- Table structure for wf_taskhistory
-- ----------------------------
DROP TABLE IF EXISTS `wf_taskhistory`;
CREATE TABLE `wf_taskhistory`  (
  `history_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `task_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `flow_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `instance_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `process_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `node_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nodename` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nodetype` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `previous_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `previousname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `authorize_user_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `authorizen_userame` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `result` int(255) NOT NULL COMMENT '1 同意 2 拒绝',
  `opinion` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `createtime` datetime(0) NULL DEFAULT NULL,
  `create_userid` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `create_username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`history_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wf_taskhistory
-- ----------------------------
INSERT INTO `wf_taskhistory` VALUES ('1335227593745633280', '', '1306861995051585536', '1335227549910962176', '1335227273896398848', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-05 22:20:40', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335227663446577152', '1335227663333330944', '1306861995051585536', '1335227549910962176', '1335227273896398848', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', 'stepnode', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '1', '超级管理员', 1, '审批人包含本人，自动通过', '2020-12-05 22:20:57', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335227663446577153', '', '1306861995051585536', '1335227549910962176', '1335227273896398848', '444ae837-e699-ab72-f58d-31991db832e9', '结束', 'endround', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '', '', 0, '流程结束', '2020-12-05 22:20:57', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335243219591630848', '', '1306861995051585536', '1335243218836656128', '1335243218379476992', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-05 23:22:46', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335243219688099840', '1335243219625185280', '1306861995051585536', '1335243218836656128', '1335243218379476992', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', 'stepnode', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '1', '超级管理员', 1, '审批人包含本人，自动通过', '2020-12-05 23:22:46', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335243219688099841', '', '1306861995051585536', '1335243218836656128', '1335243218379476992', '444ae837-e699-ab72-f58d-31991db832e9', '结束', 'endround', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '', '', 0, '【流程结束】', '2020-12-05 23:22:46', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335244434534699008', '', '1306861995051585536', '1335244434073325568', '1335244433901359104', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-05 23:27:35', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1335253236612993024', '1335244434534699009', '1306861995051585536', '1335244434073325568', '1335244433901359104', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', 'stepnode', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '1', '超级管理员', 1, '【审批】', '2020-12-06 00:02:34', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335253236650741760', '', '1306861995051585536', '1335244434073325568', '1335244433901359104', '444ae837-e699-ab72-f58d-31991db832e9', '结束', 'endround', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', '', '', 0, '【流程结束】', '2020-12-06 00:02:34', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335254301555167232', '', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-06 00:06:48', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1335467778697728000', '1335465676869079040', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '审批', '', '', 0, '超级管理员 通过流程监控调整 当前待办任务为【测试用户2审批】 调整后为【超级管理员审批】', '2020-12-06 14:15:05', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335469210154962946', '1335467778643202048', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', 'stepnode', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '', '', 0, '超级管理员 通过流程监控调整 当前待办任务为【超级管理员审批】 调整后为【测试用户2审批】', '2020-12-06 14:20:46', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335469836964335616', '1335469210154962944', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', '', '', 0, '超级管理员 通过流程监控调整 当前待办任务为【测试用户2审批】 调整后为【超级管理员审批】', '2020-12-06 14:23:16', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335473048383524864', '1335469754865029120', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', 'stepnode', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '', '', 0, '超级管理员 通过流程监控调整 当前待办任务为【超级管理员审批】 调整后为【测试用户2审批】', '2020-12-06 14:36:01', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335474954497560576', '1335473032252231680', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', '', '', 0, '超级管理员 通过流程监控调整该流程至最新版本 调整后为【测试用户2审批】', '2020-12-06 14:43:36', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335476747453468672', '1335474954468200448', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '', '', 0, '超级管理员 通过流程监控调整该流程至最新版本 调整后为【测试用户2审批】', '2020-12-06 14:50:43', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335476855498739712', '1335476747449274368', '1306861995051585536', '1335254301001519104', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '1306793948387348480', '测试用户2', 2, '【审批】不同意', '2020-12-06 14:51:09', '1306793948387348480', '测试用户2', '');
INSERT INTO `wf_taskhistory` VALUES ('1335476855544877056', '', '1306861995051585536', '1335254301001519104', '1335254300582088704', '444ae837-e699-ab72-f58d-31991db832e9', '结束', 'endround', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '', '', 0, '【流程结束】', '2020-12-06 14:51:09', '1306793948387348480', '测试用户2', '');
INSERT INTO `wf_taskhistory` VALUES ('1335477866472804352', '', '1306861995051585536', '1335477865671692288', '1335254300582088704', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-06 14:55:10', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1335478030407176192', '1335477866472804353', '1306861995051585536', '1335477865671692288', '1335254300582088704', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', 'stepnode', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', '', '', 0, '超级管理员 通过流程监控调整 当前待办任务为【测试用户2审批】 调整后为【超级管理员审批】', '2020-12-06 14:55:49', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335482075595280384', '1335478030402981888', '1306861995051585536', '1335477865671692288', '1335254300582088704', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', 'stepnode', 'af55b2d7-1e63-2434-9611-4104893a1da0', '测试用户2审批', '1', '超级管理员', 1, '【审批】', '2020-12-06 15:11:54', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1335482217769603072', '', '1306861995051585536', '1335477865671692288', '1335254300582088704', '444ae837-e699-ab72-f58d-31991db832e9', '结束', 'endround', 'fcde8608-a060-3fb7-b066-71e74d577d2c', '超级管理员审批', '', '', 0, '【流程结束】', '2020-12-06 15:12:27', '1', '超级管理员', '');
INSERT INTO `wf_taskhistory` VALUES ('1336677443364327424', '', '1306861995051585536', '1336677442303168512', '1336677441883738112', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-09 22:21:51', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1336677516676567040', '', '1306861995051585536', '1336677515871260672', '1336677515674128384', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-09 22:22:09', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1336681153289129984', '', '1306861995051585536', '1336681152248942592', '1336677441883738112', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-09 22:36:36', '1306793709295243264', '测试用户', '');
INSERT INTO `wf_taskhistory` VALUES ('1336681212277821440', '', '1306861995051585536', '1336681211438960640', '1336677515674128384', 'e3b1dc31-cc41-2702-5de8-49bf8fd20e62', '开始', 'startround', '0', '', '', '', 0, '【发起流程】', '2020-12-09 22:36:50', '1306793709295243264', '测试用户', '');

SET FOREIGN_KEY_CHECKS = 1;
