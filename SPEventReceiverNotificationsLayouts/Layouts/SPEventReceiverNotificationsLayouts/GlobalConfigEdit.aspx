<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalConfigEdit.aspx.cs" Inherits="SPEventReceiverNotificationsLayouts.Layouts.SPEventReceiverNotificationsLayouts.GlobalConfigEdit" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Table ID="NotificationsParamsTable" runat="server" CssClass="ms-viewheadertr" style="margin-bottom:20px;margin-top:20px;">
<%--        <asp:TableRow ID="TableRow5" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    Path to PS script (for test button)
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                </div></asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="PSScriptPathTextBox1" runat="server" Width="300px"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>--%>
        <asp:TableRow ID="TableRow4" runat="server" BackColor="White" >
            <asp:TableCell>
                <h3 class="ms-standardheader ms-inputformheader">
                    Site all lists configs
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:HyperLink ID="AllConfigs" runat="server" Text="AllConfigs" NavigateUrl="SiteAllConfigs.aspx"></asp:HyperLink>
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
        <asp:TableRow ID="TableRow1" runat="server" BackColor="White" >
            <asp:TableCell Width="500px">
                <h3 class="ms-standardheader ms-inputformheader">
                    AccountsExclusionsRegexp
                </h3>
                <div class="ms-descriptiontext ms-inputformdescription">
                    Accounts exclusions regexp
                </div>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxAccountsExclusionsRegexp" runat="server" Width="380"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="ButtonOK" runat="server" Text="OK" OnClick="ButtonOK_EventHandler"/>
    <asp:Button ID="ButtonCANCEL" runat="server" Text="Cancel" OnClick="ButtonCANCEL_EventHandler"/>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Common config settings
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Common config settings
</asp:Content>
