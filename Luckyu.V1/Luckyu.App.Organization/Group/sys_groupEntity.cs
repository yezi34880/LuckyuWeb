using SqlSugar;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
	/// <summary>
	///  sys_group   
	/// </summary>
	[SugarTable( "sys_group")]
	public class sys_groupEntity 
	{
		#region 属性

		/// <summary>
		///  group_id   
		/// </summary>
		[SugarColumn(IsPrimaryKey = true)]
		public string group_id { get; set; }

		/// <summary>
		///  groupcode   
		/// </summary>
		public string groupcode { get; set; }

		/// <summary>
		///  groupname   
		/// </summary>
		public string groupname { get; set; }

		/// <summary>
		///  sort   
		/// </summary>
		public int sort { get; set; }

		/// <summary>
		///  remark   
		/// </summary>
		public string remark { get; set; }

		/// <summary>
		///  create_userid   
		/// </summary>
		public string create_userid { get; set; }

		/// <summary>
		///  create_username   
		/// </summary>
		public string create_username { get; set; }

		/// <summary>
		///  createtime   
		/// </summary>
		public DateTime? createtime { get; set; }

		/// <summary>
		///  edittime   
		/// </summary>
		public DateTime? edittime { get; set; }

		/// <summary>
		///  edit_userid   
		/// </summary>
		public string edit_userid { get; set; }

		/// <summary>
		///  edit_username   
		/// </summary>
		public string edit_username { get; set; }

		/// <summary>
		///  is_delete   
		/// </summary>
		public int is_delete { get; set; }

		/// <summary>
		///  delete_userid   
		/// </summary>
		public string delete_userid { get; set; }

		/// <summary>
		///  delete_username   
		/// </summary>
		public string delete_username { get; set; }

		/// <summary>
		///  deletetime   
		/// </summary>
		public DateTime? deletetime { get; set; }

		/// <summary>
		///  is_enable   
		/// </summary>
		public int is_enable { get; set; }

		#endregion

		#region 方法
		public void Create(UserModel loginInfo)
		{
			if (this.group_id.IsEmpty())
            {
				this.group_id = SnowflakeHelper.NewCode();
            }
			this.createtime = DateTime.Now;
			this.create_userid = loginInfo.user_id;
			this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
		}
		public void Modify(string keyValue, UserModel loginInfo)
		{
			this.group_id = keyValue;
			this.edittime = DateTime.Now;
			this.edit_userid = loginInfo.user_id;
			this.edit_username = $"{loginInfo.realname}-{loginInfo.loginname}";
		}
		public void Remove(UserModel loginInfo)
		{
			this.is_delete = 1;
			this.deletetime = DateTime.Now;
			this.delete_userid = loginInfo.user_id;
			this.delete_username = $"{loginInfo.realname}-{loginInfo.loginname}";
		}
		#endregion

	}
}
