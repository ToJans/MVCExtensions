<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tasks.Core.ViewModel.Tasks.VMIndex>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <div>
        <h1>Task list</h1>
        <%= Html.Grid(Model.AllTasks)
                .Columns(c=> {
                    c.For(t => t.Name);
                    c.For(t => t.Description);
                    c.For(t => Html.ActionLink(t.AL_Status())).Named("Status").DoNotEncode();
                    c.For(t => Html.ActionLink(t.AL_Edit())).Named("Edit").DoNotEncode();
                    c.For(t => Html.ActionLink(t.AL_Delete())).Named("Delete").DoNotEncode();
                }).Empty("No tasks yet") 
        %>
        
        <hr />
        <h3>Add a new task</h3>
        <% using (Html.BeginForm(Model.AL_AddTask)) { %>
            Name<br />
            <%=Html.TextBox("Name") %><br />
            Description<br />
            <%=Html.TextArea("Description") %><br />
            <%=Html.SubmitButton(Model.AL_AddTask) %><br />
        <% }%> 
    
    </div>
</body>
</html>
