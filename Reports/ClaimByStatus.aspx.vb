Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_ClaimByStatus
    Inherits System.Web.UI.Page
    Private Sub ReportView_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim crystalReport As New ReportDocument()
        crystalReport.Load(Server.MapPath("ClaimByStatus.rpt"))
        crystalReport.SetDatabaseLogon("sa", "B1Admin", "SAPSERVER4", "FocusClaimSystem")
        crystalReport.SetParameterValue("@status", ddlStatus.SelectedValue)
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        CrystalReportViewer1.HasToggleGroupTreeButton = False
        CrystalReportViewer1.HasToggleParameterPanelButton = False
        CrystalReportViewer1.ReportSource = crystalReport



    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''RESPONSE.WRITE(Server.MapPath("ClaimByStatus.rpt"))
        'Dim crystalReport As New ReportDocument()
        'crystalReport.Load(Server.MapPath("ClaimByStatus.rpt"))
        'crystalReport.SetDatabaseLogon("sa", "B1Admin", "SAPSERVER", "FocusClaimSystem")
        'crystalReport.SetParameterValue("@status", ddlStatus.SelectedValue)
        'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        'CrystalReportViewer1.HasToggleGroupTreeButton = False
        'CrystalReportViewer1.HasToggleParameterPanelButton = False
        'CrystalReportViewer1.ReportSource = crystalReport
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim crystalReport As New ReportDocument()
        crystalReport.Load(Server.MapPath("ClaimByStatus.rpt"))
        crystalReport.SetDatabaseLogon("sa", "B1Admin", "SAPSERVER4", "FocusClaimSystem")
        crystalReport.SetParameterValue("@status", ddlStatus.SelectedValue)
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        CrystalReportViewer1.HasToggleGroupTreeButton = False
        CrystalReportViewer1.HasToggleParameterPanelButton = False
        CrystalReportViewer1.ReportSource = crystalReport
    End Sub
End Class
