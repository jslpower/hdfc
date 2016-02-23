/// <reference path="/js/jquery.js" />

var loadXls = {
    array: new Array(),
    sel: null,
    box: null,
    selval: null,
    ///初使化arr二维数组， box:table容器 ，sel列表选择器
    init: function(arr, box, sel) {
        var that = this;
        that.array = arr;
        that.sel = sel;
        that.box = $(box);
        that._createTable();
        that._bindSelect();
    },
    //建表
    _createTable: function() {
        var that = this;
        var html = [];
        if (that.array.length > 0) {
            html.push("<table width=\"100%\" border=\"0\"  cellpadding=\"0\" cellspacing=\"1\" id=\"xlstable\" >");
            html.push("<tr bgcolor=\"#BDDCF4\">");
            html.push("<th width=\"35px\">序号</th>");
            for (var j = 0; j < that.array[0].length; j++) {
                html.push("<th>" + that.array[0][j] + "</th>");
            }
            html.push("</tr>");
            for (var i = 1; i < that.array.length; i++) {
                html.push("<tr tid='" + i + "' bgcolor=\"#e3f1fc\">");
                html.push("<td><input type=\"checkbox\" class=\"checkbox\" /></td>");
                for (var j = 0; j < that.array[i].length; j++) {
                    html.push("<td>" + that.array[i][j] + "</td>");
                }
                html.push("</tr>");
            }
            html.push("</table>");
            that.box.html(html.join(""));
        } else {
            that.box.html("");
        }
    },
    //绑定设置返回选择数组
    bindIndex: function(newarr) {
        var that = this;
        var retarr = [];
        $("#xlstable").find("input.checkbox").each(function() {
            if ($(this).attr("checked")) {
                var _this = $(this);
                var tid = parseInt(_this.parent().parent().attr("tid"));
                var rowlist = [];
                for (var t = 0; t < newarr.length; t++) {
                    rowlist.push((that.array[tid][newarr[t]]));
                }
                retarr.push(rowlist);
            }
        });
        return retarr;
    },
    //初始化绑定
    _bindSelect: function() {
        var that = this;
        if (that.sel) {
            var selhtml = [];
            if (that.array.length > 0) {
                selhtml.push("<option value='-1'>请选择</option>");
                for (var j = 0; j < that.array[0].length; j++) {
                    selhtml.push("<option value='" + j + "' text='" + that.array[0][j] + "'>" + that.array[0][j] + "</option>");
                }
            }
            selhtml = selhtml.join("");
            $(that.sel).html(selhtml);
            $(that.sel).each(function() {
                var _tha = $(this);
                var _text = $.trim(_tha.prev("label").text().replace("：", ""));
                setTimeout(function() {
                    if (_tha.find("option[text='" + _text + "']").length > 0) {
                        _tha.find("option[text='" + _text + "']").attr("selected", true);
                    }
                }, 200);
            });

        }

        //$("body").hide().show();
    },

    //全选清空
    selectAll: function(o) {
        $("#xlstable").find("input.checkbox").attr("checked", o ? true : false);
        return false;
    },
    //反选
    selectback: function() {
        $("#xlstable").find("input.checkbox").each(function() {
            var _this = $(this);
            _this.attr("checked", _this.attr("checked") ? false : true);
        })
        return false;
    }
}
