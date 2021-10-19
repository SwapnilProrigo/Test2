<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.vb" Inherits="SalesOrderApplication_VB._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Sales Order Application</h2>
<p>Sales Order#:&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Find" />
        &nbsp;&nbsp;<asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
            Text="Edit" />
        &nbsp;<asp:Button ID="Button3" runat="server" Text="Delete" 
        onclick="Button3_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="StatusMessage" runat="server"></asp:Label>
    &nbsp;&nbsp;</p>
    <asp:Panel ID="Panel2" runat="server" Direction="LeftToRight" 
        GroupingText="Sales Order Info" style="margin-bottom: 0px" 
    Width="433px">
        <asp:Label ID="Label1" runat="server" Text="Status:"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="PO#:"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="TextBox3" runat="server" Width="134px"></asp:TextBox>
        <br />
        <br />
        Manager:
        <asp:TextBox ID="TextBox4" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" GroupingText="Shipping Info" 
        Width="432px">
        Shipping Method:&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>Federal Express</asp:ListItem>
            <asp:ListItem>UPS</asp:ListItem>
         </asp:DropDownList>
        <br />
        <br />
        Multiple Shipments:&nbsp;
        <asp:CheckBox ID="CheckBox1" runat="server" />
        <br />
    </asp:Panel>
    
        Total Cost: 
        <asp:TextBox ID="TextBox5" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
    
    </asp:Content>
