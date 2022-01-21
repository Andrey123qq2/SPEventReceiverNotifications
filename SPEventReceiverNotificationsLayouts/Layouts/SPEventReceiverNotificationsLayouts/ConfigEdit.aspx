<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigEdit.aspx.cs" Inherits="SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts.ConfigEdit" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <br/>
    <asp:Label ID="InfoLabel1" runat="server"></asp:Label>
    <br/>
    <asp:Table ID="TableMainParams" runat="server" CssClass="ms-viewheadertr" style="margin-bottom:20px;margin-top:20px;">
        <asp:TableRow ID="TableRow3" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    Config name
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Configuration name
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="ConfigNameTextBox" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow1" runat="server" BackColor="White" ForeColor="#000">
            <asp:TableCell Width="200">
                <h3 class="ms-standardheader ms-inputformheader">
                    Enable
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Enable/disable configuration
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:CheckBox ID="EnableCheckBox" runat="server"></asp:CheckBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow24" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    Notes
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Configuation notes
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="NotesTextBox" runat="server" Width="380" TextMode="MultiLine" Height="100"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    SendType
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Notification type
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="DropDownListSendType" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow4" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    EventType
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Event type
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="DropDownListEventType" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow13" runat="server" BackColor="White" ForeColor="#000">
            <asp:TableCell Width="200">
                <h3 class="ms-standardheader ms-inputformheader">
                    SingleMode
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Notification is sent to each recipient separately (not all in one message)
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:CheckBox ID="CheckBoxSingleRecipientMode" runat="server"></asp:CheckBox>
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow ID="TableRow9" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    DisableGlobalAccountExclusion
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Disable global account exclusion
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:CheckBox ID="CheckBoxDisableGlobalAccountExclusion" runat="server"></asp:CheckBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow5" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    ContentTypeExclusions
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Content type exclusions
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:CheckBoxList ID="CheckBoxListContentTypeFilter" runat="server"></asp:CheckBoxList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow6" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    ToMails
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    To mails adresses (separeted by ;)
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxToMails" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow7" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    ССMails
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    CC mails adresses (separeted by ;)
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxCCMails" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>        
        <asp:TableRow ID="TableRow8" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    BССMails
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    BCC mails adresses (separeted by ;)
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxBCCMails" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow10" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    ExcludedManagersMails
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Mails to exclude from NotifyManagers parameter (separeted by ;)
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxExcludedMails" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow11" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    ReplyToTemplate
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    ReplyTo address template. <br/>
                    Supported macros: {ITEMID} - replaced by item id, {LISTGUIDBASE64} - list guid converted to BASE64 string (w/o suffix ==)<br/>
                    Example: SharePoint-Comments {LISTGUIDBASE64}_{ITEMID}_x041ax043ex043cx043cx04Comments@test.local<br/>
                    Local part should not be greater then 64 symbols.
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxReplyToTemplate" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow12" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    SubjectTemplate
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Subject template. <br/>
                    Supported macros: {FIELDNAME} - replaced by field value
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxSubjectTemplate" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow23" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    BodyTemplate
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    HTML body Template. <br/>
                    Supported macros in body: {ITEMURL} - item URLs, {ATTACHURL} - attachment url, {ATTACHNAME} - attachment name, {EDITOR} - editor display name, {RELATEDITEMURL} - url of related item, {RELATEDITEMTITLE} - title of related item. <br/>
                    Supported macros in field template: {NAME} - field name, {PREVVALUE} - previous field value, {NEWVALUE} - new/current field value
                    <br/>
                    <br/>
                    <asp:Button ID="ButtonBodyTemplateCreate" runat="server" Text="IncludeInBody" OnClick="ButtonBodyTemplateCreate_Click"/>
                </div>
                </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxBodyTemplate" runat="server" Width="850" TextMode="MultiLine" Height="350"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow32" runat="server" BackColor="White"  ForeColor="#000" >
            <asp:TableCell>Config last modified date</asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="ConfModified" CssClass="notes" runat="server" Text=""></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow14" runat="server" BackColor="White"  ForeColor="#000" >
            <asp:TableCell>Config last modified by</asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="ConfModifiedBy" CssClass="notes" runat="server" Text=""></asp:Label>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>

    <SharePoint:SPGridView ID="TableFields" runat="server" AutoGenerateColumns="false">
        <RowStyle BackColor="#f6f7f8" Height="30px" HorizontalAlign="Left" />
        <AlternatingRowStyle BackColor="White" ForeColor="#000" Height="30px" HorizontalAlign="Left" />
        <HeaderStyle Font-Bold="true" HorizontalAlign="Left" CssClass="ms-viewheadertr" />
        <HeaderStyle />
        <Columns>
            <asp:TemplateField HeaderText="AddToBody" HeaderStyle-Width="50px">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBoxForBodyTemplate" runat="server" />
                </ItemTemplate> 
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Field" HeaderStyle-Width="200px">
                <ItemTemplate>
                    <asp:Label ID="LabelFieldName" runat="server" Text='<%# Eval("FieldName") %>'></asp:Label>
                </ItemTemplate> 
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Field IntName" HeaderStyle-Width="500px">
                <ItemTemplate>
                    <asp:Label ID="LabelFieldIntName" runat="server" Text='<%# Eval("FieldIntName") %>'></asp:Label>
                </ItemTemplate> 
            </asp:TemplateField> 
            
            <%--IDs of elements should be same as props in SPListsNotificationsConfigItem class--%>
            <asp:TemplateField HeaderText="ToFields" ControlStyle-Width="100">
                <ItemTemplate>
                    <asp:CheckBox ID="ToFields" runat="server" AutoPostBack="false" Checked='<%# Eval("ToFields") %>' Visible='<%# Eval("IsUserField") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CCFields" ControlStyle-Width="100">
                <ItemTemplate>
                    <asp:CheckBox ID="CCFields" runat="server" AutoPostBack="false" Checked='<%# Eval("CCFields") %>' Visible='<%# Eval("IsUserField") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="BCCFields" ControlStyle-Width="100">
                <ItemTemplate>
                    <asp:CheckBox ID="BCCFields" runat="server" AutoPostBack="false" Checked='<%# Eval("BCCFields") %>' Visible='<%# Eval("IsUserField") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="NotifyManagers" ControlStyle-Width="100">
                <ItemTemplate>
                    <asp:CheckBox  ID="MailFieldsManagers" runat="server" AutoPostBack="false" Checked='<%# Eval("MailFieldsManagers") %>' Visible='<%# Eval("IsUserField") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </SharePoint:SPGridView>
    <br/>
    <asp:Button ID="ButtonOK" runat="server" Text="OK" OnClick="ButtonOK_EventHandler"/>
    <asp:Button ID="ButtonCANCEL" runat="server" Text="Cancel" OnClick="ButtonCANCEL_EventHandler"/>
    <span style="padding-left: 150px;">
        <asp:Button ID="ButtonREMOVE" runat="server" Text="Remove" OnClick="ButtonREMOVE_EventHandler"/>
    </span>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Immidiate notifications config
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
    <a id="ListConfigsLink" runat="server">All List Configs</a>
    <span>
        <span style="height:16px;width:16px;position:relative;display:inline-block;overflow:hidden;">
            <img src="/_layouts/15/images/spcommon.png?rev=43#ThemeKey=spcommon" alt=":" style="position:absolute;left:-109px !important;top:-232px !important;"/>
        </span>
    </span>
    <span>immidiate notifications config</span>
</asp:Content>