<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tasks.Core.ViewModel.Tasks.VMEdit>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit</title>
</head>
<body>
    <div>
        <h3>Edit this task</h3>
        <% using (Html.BeginForm(Model.AL_PostEdit)) { %>
            Name<br />
            <%=Html.EditorFor(m=>m.Name) %><br />
            Description<br />
            <%=Html.TextArea("Description",Model.Description)%><br />
            <%=Html.SubmitButton(Model.AL_PostEdit) %><br />
        <% }%> 
        
        <%=Html.ActionLink(Model.AL_CancelEdit) %>
    
    </div>
</body>
</html>
