# MySQL-Front 4.2  (Build 2.7)

/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE */;
/*!40101 SET SQL_MODE='' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES */;
/*!40103 SET SQL_NOTES='ON' */;


# Host: localhost    Database: db_sem
# ------------------------------------------------------
# Server version 5.0.67-community-nt

DROP DATABASE IF EXISTS `db_sem`;
CREATE DATABASE `db_sem` /*!40100 DEFAULT CHARACTER SET gbk */;
USE `db_sem`;

#
# Table structure for table tb_accessright
#

CREATE TABLE `tb_accessright` (
  `id` int(11) NOT NULL auto_increment,
  `fk_lrid` int(11) NOT NULL,
  `fk_uid` varchar(255) NOT NULL,
  `remark` varchar(255) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_accessright
#


#
# Table structure for table tb_activemf
#

CREATE TABLE `tb_activemf` (
  `Id` int(11) NOT NULL auto_increment,
  `fk_gid` int(11) NOT NULL default '0',
  `mfno` varchar(512) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_activemf
#


#
# Table structure for table tb_admin
#

CREATE TABLE `tb_admin` (
  `id` int(11) NOT NULL auto_increment,
  `fk_uid` char(255) NOT NULL,
  `name` char(255) default NULL,
  PRIMARY KEY  (`id`,`fk_uid`),
  UNIQUE KEY `id` (`id`),
  KEY `admin_fk_uid` (`fk_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_admin
#

INSERT INTO `tb_admin` VALUES (1,'10000','ADMIN');

#
# Table structure for table tb_class
#

CREATE TABLE `tb_class` (
  `id` smallint(6) NOT NULL auto_increment,
  `name` varchar(255) NOT NULL,
  `remark` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_class
#

INSERT INTO `tb_class` VALUES (1,'Class',NULL);

#
# Table structure for table tb_concollect
#

CREATE TABLE `tb_concollect` (
  `Id` int(11) NOT NULL auto_increment,
  `templatecon` varchar(64) NOT NULL default '',
  `contype` int(11) default NULL,
  `modelcon` float default NULL,
  `description` varchar(64) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_concollect
#


#
# Table structure for table tb_engineer
#

CREATE TABLE `tb_engineer` (
  `id` int(11) NOT NULL auto_increment,
  `fk_uid` char(255) NOT NULL,
  `name` char(255) default NULL,
  PRIMARY KEY  (`id`,`fk_uid`),
  UNIQUE KEY `id` (`id`),
  KEY `engineer_fk_uid` (`fk_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_engineer
#


#
# Table structure for table tb_event
#

CREATE TABLE `tb_event` (
  `id` int(11) NOT NULL auto_increment,
  `uid` varchar(255) default NULL,
  `ip` varchar(255) NOT NULL,
  `time` datetime default NULL,
  `eventlog` varchar(255) NOT NULL,
  `remark` varchar(255) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_event
#

INSERT INTO `tb_event` VALUES (1,'10000','192.168.1.100','2017-10-16 16:12:35','登录',NULL);

#
# Table structure for table tb_examhis
#

CREATE TABLE `tb_examhis` (
  `id` int(11) NOT NULL auto_increment,
  `groupname` varchar(255) default NULL,
  `paper` varchar(255) default NULL,
  `examtime` datetime default NULL,
  `usingtime` int(11) default NULL,
  `score` float default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_examhis
#


#
# Table structure for table tb_examlog
#

CREATE TABLE `tb_examlog` (
  `id` int(11) NOT NULL auto_increment,
  `fk_sid` int(11) NOT NULL,
  `fk_tid` int(11) NOT NULL,
  `fk_tpid` int(11) NOT NULL,
  `time` datetime default NULL,
  `content` varchar(255) default NULL,
  `grade` double default NULL,
  `subgrade` double default NULL,
  `notes` varchar(255) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_examlog
#


#
# Table structure for table tb_examuserhis
#

CREATE TABLE `tb_examuserhis` (
  `id` int(11) NOT NULL auto_increment,
  `uid` varchar(255) NOT NULL default '',
  `paper` varchar(255) default NULL,
  `examtime` datetime default NULL,
  `usingtime` int(11) default NULL,
  `score` float default NULL,
  `fng` varchar(255) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_examuserhis
#


#
# Table structure for table tb_function
#

CREATE TABLE `tb_function` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(255) NOT NULL,
  `varnumber` int(11) default '0',
  `vartype` varchar(255) default NULL,
  `description` varchar(255) default NULL,
  `code` text,
  PRIMARY KEY  (`id`,`name`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_function
#

INSERT INTO `tb_function` VALUES (1,'Equd',1,'0','Equd','//Equd\r\n//1\r\n//0\r\n//Equd\r\nfunction Equd(D1)\r\n{\r\n  var result = 0;\r\n  if(D1==1)\r\n  {\r\n    result = 1;\r\n  }\r\n  return result;\r\n}\r\n');

#
# Table structure for table tb_group
#

CREATE TABLE `tb_group` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(255) NOT NULL,
  `content` varchar(255) default NULL,
  `model` varchar(255) default NULL,
  `drive` varchar(255) default NULL,
  `starid` int(11) default NULL,
  `staruser` varchar(255) default NULL,
  `starpassword` varchar(255) default NULL,
  `ip` varchar(255) default NULL,
  `port` int(11) default NULL,
  `remark` varchar(255) default NULL,
  `link` enum('YES','NO') default 'NO',
  `stateno` int(11) default '1',
  PRIMARY KEY  (`id`,`name`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_group
#

INSERT INTO `tb_group` VALUES (1,'南通1组','600MW机组','D:\\STAR_NT\\mdl_NT\\NT01.mdl','D:\\STAR_NT',1,'star','star','128.1.140.20',9999,'','NO',5);

#
# Table structure for table tb_groupinfo
#

CREATE TABLE `tb_groupinfo` (
  `id` int(11) NOT NULL auto_increment,
  `fk_gid` int(11) NOT NULL,
  `fk_sid` int(11) NOT NULL,
  `remark` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_groupinfo
#

INSERT INTO `tb_groupinfo` VALUES (1,1,1,'');

#
# Table structure for table tb_groupstatus
#

CREATE TABLE `tb_groupstatus` (
  `Id` int(11) NOT NULL auto_increment,
  `fk_gid` int(11) NOT NULL default '0',
  `curtime` datetime default NULL,
  `modeltime` int(11) NOT NULL default '-1',
  `modelstatus` int(11) NOT NULL default '0',
  `ic` int(11) NOT NULL default '-1',
  `mf` varchar(512) default NULL,
  `scn` varchar(128) default NULL,
  `lt` varchar(128) default NULL,
  `gt` int(11) NOT NULL default '-1',
  `curve` varchar(512) default NULL,
  `exam` int(11) NOT NULL default '-1',
  `examtime1` datetime default NULL,
  `examtime2` datetime default NULL,
  `param` varchar(128) default NULL,
  `alarma` varchar(512) default NULL,
  `alarmd` varchar(512) default NULL,
  `int1` int(11) NOT NULL default '-1',
  `int2` int(11) NOT NULL default '-1',
  `int3` int(11) NOT NULL default '-1',
  `int4` int(11) NOT NULL default '-1',
  `string1` varchar(642) default NULL,
  `string2` varchar(642) default NULL,
  `string3` varchar(642) default NULL,
  `string4` varchar(642) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_groupstatus
#


#
# Table structure for table tb_hoststatus
#

CREATE TABLE `tb_hoststatus` (
  `Id` int(11) NOT NULL auto_increment,
  `hostip` varchar(26) default NULL,
  `lastdate` varchar(24) default NULL,
  `curdate` varchar(24) default NULL,
  `status` int(11) default '0',
  `username` varchar(255) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_hoststatus
#

INSERT INTO `tb_hoststatus` VALUES (1,'127.0.0.1','2017-10-15','2017-10-16',1,NULL);

#
# Table structure for table tb_localprogram
#

CREATE TABLE `tb_localprogram` (
  `id` int(11) NOT NULL auto_increment,
  `fk_lrid` int(11) NOT NULL,
  `program` varchar(255) default NULL,
  `path` varchar(255) default NULL,
  `parameter` varchar(255) default NULL,
  `monnetpc` varchar(255) default NULL,
  `port` int(11) default NULL,
  `maxpva` int(11) default NULL,
  `maxpvd` int(11) default NULL,
  `netdisp` enum('YES','NO') default NULL,
  `sysname` varchar(32) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_lrid` (`fk_lrid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_localprogram
#

INSERT INTO `tb_localprogram` VALUES (1,1,'G:\\DCS\\bin\\OvationRun.exe','G:\\DCS\\bin','','G:\\DCS\\Net\\monnetpc.exe',119876,20000,20000,'NO','NT01DCS');
INSERT INTO `tb_localprogram` VALUES (2,2,'G:\\LOC_NT\\loc.mcp','G:\\LOC_NT','','C:\\Program Files (x86)\\Haoyue\\bin\\monnetpc.exe',111944,20000,20000,'NO','NT01LOC');

#
# Table structure for table tb_localrun
#

CREATE TABLE `tb_localrun` (
  `id` int(11) NOT NULL auto_increment,
  `fk_gid` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_gid` (`fk_gid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_localrun
#

INSERT INTO `tb_localrun` VALUES (1,1,'DCS','南通1组DCS');
INSERT INTO `tb_localrun` VALUES (2,1,'LOC','南通1组LOC');

#
# Table structure for table tb_login
#

CREATE TABLE `tb_login` (
  `id` int(11) NOT NULL auto_increment,
  `ip` varchar(255) NOT NULL,
  `time` datetime default NULL,
  `type` enum('IN','OUT') default NULL,
  `uid` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_login
#


#
# Table structure for table tb_netinfo
#

CREATE TABLE `tb_netinfo` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(255) NOT NULL,
  `ip` varchar(255) NOT NULL,
  `remark` varchar(255) default NULL,
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_netinfo
#


#
# Table structure for table tb_onlinetime
#

CREATE TABLE `tb_onlinetime` (
  `uid` varchar(128) default NULL,
  `loginin` time default NULL,
  `loginout` time default NULL,
  `timelen` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_onlinetime
#


#
# Table structure for table tb_operate
#

CREATE TABLE `tb_operate` (
  `id` int(11) NOT NULL,
  `fk_sid` varchar(255) NOT NULL,
  `time` datetime default NULL,
  `content` text,
  `model` varchar(255) default NULL,
  `condition` varchar(255) default NULL,
  `error` varchar(255) default NULL,
  UNIQUE KEY `id` (`id`),
  KEY `fk_sid` (`fk_sid`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_operate
#


#
# Table structure for table tb_remoteprogram
#

CREATE TABLE `tb_remoteprogram` (
  `id` int(11) NOT NULL auto_increment,
  `fk_rrid` int(11) default NULL,
  `program` varchar(255) default NULL,
  `path` varchar(255) default NULL,
  `parameter` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_rrid` (`fk_rrid`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_remoteprogram
#


#
# Table structure for table tb_remoterun
#

CREATE TABLE `tb_remoterun` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_remoterun
#


#
# Table structure for table tb_rule
#

CREATE TABLE `tb_rule` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(32) NOT NULL,
  `type` smallint(6) default NULL,
  `fk_cid` int(11) default NULL,
  `fk_eid` int(11) default NULL,
  `description` varchar(255) default NULL,
  PRIMARY KEY  (`id`,`name`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_rule
#


#
# Table structure for table tb_student
#

CREATE TABLE `tb_student` (
  `id` int(11) NOT NULL auto_increment,
  `fk_uid` varchar(255) NOT NULL,
  `name` varchar(255) default NULL,
  `age` int(11) default NULL,
  `gender` enum('FEMALE','MALE') default NULL,
  `company` varchar(255) default NULL,
  `idcard` varchar(255) default NULL,
  `fk_cid` smallint(6) default NULL,
  `notes` varchar(255) default NULL,
  `grouped` int(11) default NULL,
  PRIMARY KEY  (`id`,`fk_uid`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_uid` (`fk_uid`),
  KEY `fk_cid` (`fk_cid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_student
#

INSERT INTO `tb_student` VALUES (1,'10001','学员1',22,'MALE','-','',1,'-',1);

#
# Table structure for table tb_testlog
#

CREATE TABLE `tb_testlog` (
  `Id` int(11) NOT NULL auto_increment,
  `time` datetime default NULL,
  `examiner` varchar(255) default NULL,
  `paperfile` varchar(255) default NULL,
  `papername` varchar(255) default NULL,
  `type` int(11) default NULL,
  `note` varchar(255) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_testlog
#


#
# Table structure for table tb_trainer
#

CREATE TABLE `tb_trainer` (
  `id` int(11) NOT NULL auto_increment,
  `fk_uid` varchar(255) NOT NULL,
  `name` varchar(255) default NULL,
  PRIMARY KEY  (`id`,`fk_uid`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_uid` (`fk_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_trainer
#


#
# Table structure for table tb_trainlog
#

CREATE TABLE `tb_trainlog` (
  `id` int(11) NOT NULL auto_increment,
  `fk_sid` int(11) NOT NULL,
  `starttime` datetime default NULL,
  `endtime` datetime default NULL,
  `content` varchar(255) default NULL,
  `status` int(11) default NULL,
  `remark` varchar(255) default NULL,
  PRIMARY KEY  (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `fk_sid` (`fk_sid`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_trainlog
#


#
# Table structure for table tb_user
#

CREATE TABLE `tb_user` (
  `id` int(11) NOT NULL auto_increment,
  `uid` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` enum('TRAINER','GUEST','STUDENT','ENGINEER','ADMIN') NOT NULL default 'GUEST',
  `email` varchar(255) default NULL,
  `status` tinyint(4) default NULL,
  `question` varchar(255) default NULL,
  `answer` varchar(255) default NULL,
  `remark` varchar(255) default NULL,
  `online` enum('ON','OFF') NOT NULL default 'OFF',
  PRIMARY KEY  (`id`,`uid`),
  UNIQUE KEY `uid` (`uid`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_user
#

INSERT INTO `tb_user` VALUES (1,'10000','ADMIN','ADMIN',NULL,1,NULL,NULL,NULL,'OFF');

#
# Table structure for table tb_userstatus
#

CREATE TABLE `tb_userstatus` (
  `id` int(11) NOT NULL auto_increment,
  `fk_uid` varchar(255) NOT NULL default '',
  `fk_gid` int(11) default NULL,
  `status` int(11) default NULL,
  `time` datetime default NULL,
  `notes` varchar(255) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_userstatus
#


#
# Table structure for table tb_varcollect
#

CREATE TABLE `tb_varcollect` (
  `Id` int(11) NOT NULL auto_increment,
  `templatevar` varchar(64) NOT NULL default '',
  `vartype` int(11) default NULL,
  `modelvar` varchar(64) default NULL,
  `description` varchar(64) default NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=gbk;

#
# Dumping data for table tb_varcollect
#


#
# View structure for view view_examlog
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_examlog` AS select `tb_examlog`.`time` AS `time`,`tb_examlog`.`content` AS `content`,`tb_examlog`.`grade` AS `grade`,`tb_examlog`.`subgrade` AS `subgrade`,`tb_examlog`.`notes` AS `notes`,`tb_student`.`name` AS `name`,`tb_user`.`uid` AS `uid`,`tb_examlog`.`fk_tpid` AS `fk_tpid`,`tb_student`.`fk_uid` AS `fk_uid`,(`tb_examlog`.`grade` + ifnull(`tb_examlog`.`subgrade`,0)) AS `score`,`tb_examlog`.`id` AS `id` from ((`tb_examlog` join `tb_student`) join `tb_user`) where ((`tb_examlog`.`fk_sid` = `tb_student`.`id`) and (`tb_examlog`.`fk_tid` = `tb_user`.`id`)) order by `tb_examlog`.`id`;

#
# View structure for view view_groupinfo
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_groupinfo` AS select `tb_group`.`name` AS `groupname`,`tb_group`.`content` AS `content`,`tb_group`.`model` AS `model`,`tb_group`.`drive` AS `drive`,`tb_group`.`staruser` AS `staruser`,`tb_group`.`ip` AS `ip`,`tb_group`.`port` AS `port`,`tb_student`.`name` AS `studentname`,`tb_groupinfo`.`fk_gid` AS `fk_gid`,`tb_groupinfo`.`fk_sid` AS `fk_sid`,`tb_student`.`fk_uid` AS `fk_uid` from ((`tb_group` join `tb_student`) join `tb_groupinfo`) where ((`tb_group`.`id` = `tb_groupinfo`.`fk_gid`) and (`tb_student`.`id` = `tb_groupinfo`.`fk_sid`));

#
# View structure for view view_localprogram
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_localprogram` AS select `tb_localprogram`.`id` AS `id`,`tb_localrun`.`name` AS `name`,`tb_localprogram`.`program` AS `program`,`tb_localprogram`.`path` AS `path`,`tb_localprogram`.`parameter` AS `parameter`,`tb_localprogram`.`monnetpc` AS `monnetpc`,`tb_localprogram`.`port` AS `port`,`tb_localprogram`.`maxpva` AS `maxpva`,`tb_localprogram`.`maxpvd` AS `maxpvd`,`tb_localprogram`.`netdisp` AS `netdisp`,`tb_localprogram`.`sysname` AS `sysname` from (`tb_localrun` join `tb_localprogram`) where (`tb_localrun`.`id` = `tb_localprogram`.`fk_lrid`);

#
# View structure for view view_localrun
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_localrun` AS select `tb_localrun`.`id` AS `id`,`tb_localrun`.`name` AS `name`,`tb_localrun`.`description` AS `description`,`tb_group`.`name` AS `groupname` from (`tb_localrun` join `tb_group`) where (`tb_localrun`.`fk_gid` = `tb_group`.`id`);

#
# View structure for view view_student
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_student` AS select `tb_student`.`id` AS `id`,`tb_student`.`fk_uid` AS `fk_uid`,`tb_student`.`name` AS `name`,`tb_student`.`age` AS `age`,`tb_student`.`gender` AS `gender`,`tb_student`.`company` AS `company`,`tb_student`.`idcard` AS `idcard`,`tb_student`.`notes` AS `notes`,`tb_student`.`grouped` AS `grouped`,`tb_class`.`name` AS `classname`,`tb_user`.`password` AS `password`,`tb_user`.`role` AS `role`,`tb_user`.`email` AS `email`,`tb_user`.`status` AS `status`,`tb_user`.`question` AS `question`,`tb_user`.`answer` AS `answer`,`tb_user`.`remark` AS `remark`,`tb_user`.`online` AS `online` from ((`tb_student` join `tb_class`) join `tb_user`) where ((`tb_student`.`fk_cid` = `tb_class`.`id`) and (`tb_student`.`fk_uid` = `tb_user`.`uid`));

#
# View structure for view view_trainlog
#

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_trainlog` AS select `tb_student`.`name` AS `name`,`tb_trainlog`.`fk_sid` AS `fk_sid`,`tb_trainlog`.`starttime` AS `starttime`,`tb_trainlog`.`endtime` AS `endtime`,`tb_trainlog`.`content` AS `content`,`tb_trainlog`.`status` AS `status`,`tb_trainlog`.`remark` AS `remark`,`tb_student`.`id` AS `sid`,`tb_trainlog`.`id` AS `id`,`tb_student`.`fk_uid` AS `fk_uid` from (`tb_student` join `tb_trainlog`) where (`tb_student`.`id` = `tb_trainlog`.`fk_sid`) order by `tb_trainlog`.`id`;

#
#  Foreign keys for table tb_admin
#

ALTER TABLE `tb_admin`
  ADD CONSTRAINT `admin_fk_uid` FOREIGN KEY (`fk_uid`) REFERENCES `tb_user` (`uid`)
;

#
#  Foreign keys for table tb_engineer
#

ALTER TABLE `tb_engineer`
  ADD CONSTRAINT `engineer_fk_uid` FOREIGN KEY (`fk_uid`) REFERENCES `tb_user` (`uid`)
;

#
#  Foreign keys for table tb_localprogram
#

ALTER TABLE `tb_localprogram`
  ADD CONSTRAINT `fk_lrid` FOREIGN KEY (`fk_lrid`) REFERENCES `tb_localrun` (`id`)
;

#
#  Foreign keys for table tb_localrun
#

ALTER TABLE `tb_localrun`
  ADD CONSTRAINT `fk_gid` FOREIGN KEY (`fk_gid`) REFERENCES `tb_group` (`id`)
;

#
#  Foreign keys for table tb_operate
#

ALTER TABLE `tb_operate`
  ADD CONSTRAINT `tb_operate_ibfk_1` FOREIGN KEY (`fk_sid`) REFERENCES `tb_user` (`uid`)
;

#
#  Foreign keys for table tb_remoteprogram
#

ALTER TABLE `tb_remoteprogram`
  ADD CONSTRAINT `fk_rrid` FOREIGN KEY (`fk_rrid`) REFERENCES `tb_remoterun` (`id`)
;

#
#  Foreign keys for table tb_student
#

ALTER TABLE `tb_student`
  ADD CONSTRAINT `fk_cid` FOREIGN KEY (`fk_cid`) REFERENCES `tb_class` (`id`),
  ADD CONSTRAINT `tb_student_ibfk_1` FOREIGN KEY (`fk_uid`) REFERENCES `tb_user` (`uid`)
;

#
#  Foreign keys for table tb_trainer
#

ALTER TABLE `tb_trainer`
  ADD CONSTRAINT `fk_uid` FOREIGN KEY (`fk_uid`) REFERENCES `tb_user` (`uid`)
;

#
#  Foreign keys for table tb_trainlog
#

ALTER TABLE `tb_trainlog`
  ADD CONSTRAINT `fk_sid` FOREIGN KEY (`fk_sid`) REFERENCES `tb_student` (`id`)
;


/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
