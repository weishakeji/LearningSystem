//填空题
function setQuestionLoyout5(qitem,index){
    var tm="<dd index=\""+index+"\" quesid=\""+qitem.Qus_ID+"\" type=\""+qitem.Qus_Type+"\"><div class=\"quesBox\">";
    //序号
    tm+="<div class=\"order\">"+index+"、[填空题] <span>"+qitem.Qus_Number+" 分</span></div>";
    //题干
    tm+="<div class=\"quesTitle\">"+qitem.Qus_Title+"</div>";
    //试题
    tm+="<div class=\"ansItemBox\"  index=\""+index+"\">";
    var answer=qitem.Answer;
    for(var i=0;i<answer.length;i++)
    {
        tm+="<div class=\"ansItem\" ansid=\""+answer[i].Ans_ID+"\"><div class=\"ansItemContext\">";
        tm+=String.fromCharCode(65+i)+"、&nbsp;"
        tm+="<input name=ansItem_"+index+" type=\"text\" ans=\""+answer[i].Ans_Context+"\"/>";
        tm+="</div></div>";
    }
    tm+="</div>";
    tm+="</div></dd>"
    return tm;
}
//单选题
function setQuestionLoyout1(qitem,index){
    var tm="<dd index=\""+index+"\" quesid=\""+qitem.Qus_ID+"\" type=\""+qitem.Qus_Type+"\"><div class=\"quesBox\">";
    //序号
    tm+="<div class=\"order\">"+index+"、[单选题] <span>"+qitem.Qus_Number+" 分</span></div>";
    //题干
    tm+="<div class=\"quesTitle\">"+qitem.Qus_Title+"</div>";
    //试题
    tm+="<div class=\"ansItemBox\"  index=\""+index+"\">";
    var answer=qitem.Answer;
    for(var i=0;i<answer.length;i++)
    {
        tm+="<div class=\"ansItem\" ansid=\""+answer[i].Ans_ID+"\"><div class=\"ansItemContext\">";
        tm+=String.fromCharCode(65+i)+"、&nbsp;"
        tm+=answer[i].Ans_Context;
        tm+="</div></div>";
    }
    tm+="</div>";
    tm+="</div></dd>"
    return tm;
}
//多选题
function setQuestionLoyout2(qitem,index){
    var tm="<dd index=\""+index+"\" quesid=\""+qitem.Qus_ID+"\" type=\""+qitem.Qus_Type+"\"><div class=\"quesBox\">";
    //序号
    tm+="<div class=\"order\">"+index+"、[多选题]  <span>"+qitem.Qus_Number+" 分</span></div>";
    //题干
    tm+="<div class=\"quesTitle\">"+qitem.Qus_Title+"</div>";
    //试题
    tm+="<div class=\"ansItemBox\"  index=\""+index+"\">";
    var answer=qitem.Answer;
    for(var i=0;i<answer.length;i++)
    {
        tm+="<div class=\"ansItem\" ansid=\""+answer[i].Ans_ID+"\"><div class=\"ansItemContext\">";
        tm+=String.fromCharCode(65+i)+"、&nbsp;"
        tm+=answer[i].Ans_Context;
        tm+="</div></div>";
    }
    tm+="</div>";
    tm+="</div></dd>"
    return tm;
}
//判断题
function setQuestionLoyout3(qitem,index){
    var tm="<dd index=\""+index+"\" quesid=\""+qitem.Qus_ID+"\" type=\""+qitem.Qus_Type+"\"><div class=\"quesBox\">";
    //序号
    tm+="<div class=\"order\">"+index+"、[判断题]  <span>"+qitem.Qus_Number+" 分</span></div>";
    //题干
    tm+="<div class=\"quesTitle\">"+qitem.Qus_Title+"</div>";
    //试题
    tm+="<div class=\"ansItemBox\"  index=\""+index+"\">";
    //
    tm+="<div class=\"ansItem\" ansid=\"0\"><div class=\"ansItemContext\">";
    tm+=String.fromCharCode(65+0)+"、&nbsp;"
    tm+="正确";
    tm+="</div></div>";
    tm+="<div class=\"ansItem\" ansid=\"1\"><div class=\"ansItemContext\">";
    tm+=String.fromCharCode(65+1)+"、&nbsp;"
    tm+="错误";
    tm+="</div></div>";

    tm+="</div>";
    tm+="</div></dd>"
    return tm;
}
//简答题
function setQuestionLoyout4(qitem,index){
    var tm="<dd index=\""+index+"\" quesid=\""+qitem.Qus_ID+"\" type=\""+qitem.Qus_Type+"\"><div class=\"quesBox\">";
    //序号
    tm+="<div class=\"order\">"+index+"、[简答题]  <span>"+qitem.Qus_Number+" 分</span></div>";
    //题干
    tm+="<div class=\"quesTitle\">"+qitem.Qus_Title+"</div>";
    //试题
    tm+="<div class=\"ansItemBox\"  index=\""+index+"\">";
    tm+="<textarea name=\"textfield\" class=\"ansText\"></textarea>";
    tm+="</div>";
    tm+="</div></dd>"
    return tm;
}

