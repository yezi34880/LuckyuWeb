using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using System;
using System.Collections.Generic;

namespace Luckyu.App.System
{
	/// <summary>
	///  sys_coderule   
	/// </summary>
	[Table(Name = "SYS_CODERULE")]
	public class sys_coderuleEntity 
	{
		#region 属性

		/// <summary>
		///  rule_id   
		/// </summary>
		[Column(IsPrimary = true)]
		public string rule_id { get; set; }

		/// <summary>
		///  rulecode   
		/// </summary>
		public string rulecode { get; set; }

		/// <summary>
		///  rulename   
		/// </summary>
		public string rulename { get; set; }

		/// <summary>
		///  contentjson   
		/// </summary>
		public string contentjson { get; set; }

		/// <summary>
		///  remark   
		/// </summary>
		public string remark { get; set; }

		/// <summary>
		///  create_userId   
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
		///  deletetime   
		/// </summary>
		public DateTime? deletetime { get; set; }

		/// <summary>
		///  delete_username   
		/// </summary>
		public string delete_username { get; set; }

		/// <summary>
		///  is_enable   
		/// </summary>
		public int is_enable { get; set; }

		#endregion

		#region 方法
		public void Create(UserModel loginInfo)
		{
			this.rule_id = SnowflakeHelper.NewCode();
			this.createtime = DateTime.Now;
			this.create_userid = loginInfo.user_id;
			this.create_username = loginInfo.realname;
		}
		public void Modify(string keyValue, UserModel loginInfo)
		{
			this.rule_id = keyValue;
			this.edittime = DateTime.Now;
			this.edit_userid = loginInfo.user_id;
			this.edit_username = loginInfo.realname;
		}
		public void Remove(UserModel loginInfo)
		{
			this.is_delete = 1;
			this.deletetime = DateTime.Now;
			this.delete_userid = loginInfo.user_id;
			this.delete_username = loginInfo.realname;
		}
		#endregion

	}
}
