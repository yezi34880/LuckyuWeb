(window.webpackJsonp=window.webpackJsonp||[]).push([[24],{540:function(e,t,l){"use strict";l.r(t);l(256);var a={mounted:function(){var e=eleTree({el:".eletree6",url:"/eleTree/json/pid.json?v=2.1.15",highlightCurrent:!0,showCheckbox:!0}),t={append:function(){e.append([{label:"添加子节1",id:"a",pid:"001002002002"},{label:"添加子节点2",id:"b",pid:"001002002003"},{label:"添加子节点3",id:"ab",pid:"a"},{label:"添加子节点4",id:"ac",pid:"a",checked:!0}])},updateKeySelf_1:function(){e.updateKeySelf([{label:"桃源路11",id:"001002002002",pid:"001002001"},{label:"湖东路11",id:"001002002003",pid:"001002001"}])},updateKeySelf_2:function(){e.updateKeySelf([{label:"桃源路11",id:"001002002002",pid:"001002003"},{label:"湖东路11",id:"001002002003",pid:"001002003"}])},updateKeySelf_3:function(){e.updateKeySelf([{label:"桃源路11",id:"001002002002",pid:""},{label:"湖东路11",id:"001002002003",pid:""}])},getAllNodeData_1:function(){alert(JSON.stringify(e.getAllNodeData("p")))},getAllNodeData_2:function(){alert(JSON.stringify(e.getAllNodeData()))}};document.querySelector(".sel").onchange=function(){this.value&&t[this.value]&&t[this.value]()}}},i=l(67),d=Object(i.a)(a,(function(){var e=this.$createElement;this._self._c;return this._m(0)}),[function(){var e=this,t=e.$createElement,l=e._self._c||t;return l("div",[l("select",{staticClass:"sel"},[l("option",{attrs:{value:"",selected:""}},[e._v("请选择")]),e._v(" "),l("option",{attrs:{value:"append"}},[e._v("pid格式添加子节点")]),e._v(" "),l("option",{attrs:{value:"updateKeySelf_1"}},[e._v("pid格式修改节点")]),e._v(" "),l("option",{attrs:{value:"updateKeySelf_2"}},[e._v("pid移动节点")]),e._v(" "),l("option",{attrs:{value:"updateKeySelf_3"}},[e._v("移动到根节点")]),e._v(" "),l("option",{attrs:{value:"getAllNodeData_1"}},[e._v("获取pid格式的数据")]),e._v(" "),l("option",{attrs:{value:"getAllNodeData_2"}},[e._v("获取children格式的数据")])]),e._v(" "),l("div",{staticClass:"eletree6"})])}],!1,null,null,null);t.default=d.exports}}]);