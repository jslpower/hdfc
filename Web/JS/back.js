var serverDate;
function getNowDate(){
   serverDate.setSeconds(serverDate.getSeconds()+1);
 }
function GetMinsFromTheLastLoginTime(){
    var nowDate = serverDate, lastLoginTime;
    var c = document.cookie, value = "", key = "";
    for (var i = 0; i < c.split(";").length; i++) {
        key = c.split(";")[i].split("=")[0];
        value = c.split(";")[i].split("=")[1];
        if ($.trim(key) == "lastlogintime") {
            break;
        }else{
            value="";
        }
    }
    if(value==""){
        return 0;
    }
    var timeArr = value.split("-");
    lastLoginTime = new Date(timeArr[0], timeArr[1] - 1, timeArr[2], timeArr[3], timeArr[4], timeArr[5]);
    
    var MinMilli = 1000 * 60;
    var timeStamp = nowDate.getTime()-lastLoginTime.getTime();
    if(timeStamp<=0){
        return 0;
    }else{
        return Math.floor(timeStamp / MinMilli);
    }
}
function hideAll(str){
    var $a = $("#"+str);
    if($a.length==0){
        return;
    }
    $a.toggle();
    var b=true;
    if($a.css("display")=="none"){
        b = false;
    }else{
        b = true;
    }
    $a.prev("h2").toggleClass("firstNav",b);
}
//ie6 backgroundimage hack.
if ($.browser.msie) {
    if (parseFloat($.browser.version) <= 6) {
        try {
            document.execCommand('BackgroundImageCache', false, true);
        } catch (e) {
        }
    }
}

function queryString(val) {
    var uri = window.location.search;
    var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
    return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
}
var LoginUrl = "/login.aspx";



/*
datatype：支持json,text
*/
