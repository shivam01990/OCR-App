<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OCRExtractTable.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>OCR App</title>

    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <style>
        body {
            padding-top: 70px;
            /* Required padding for .navbar-fixed-top. Remove if using .navbar-static-top. Change if height of navigation changes. */
        }
    </style>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->


    <link href="content/bootstrap-fileinput/css/fileinput.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/fileinput.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script src="js/fileinput_locale_LANG.js"></script>
    <script type="text/javascript">
        var rootpath = '<%=Page.ResolveUrl("~")%>';
        $(document).ready(function () {
            $("#file_BrandImage").fileinput({
                uploadUrl: rootpath + 'FileUploadHandler.ashx',
                uploadAsync: true,
                allowedFileExtensions: ['jpg', 'png', 'gif']
            });

            $("#file_BrandImage").on('fileuploaded', function (event, data, previewId, index) {
                //var form = data.form, files = data.files, extra = data.extra,
                //    response = data.response, reader = data.reader; 
                $.each(data.files, function (k, obj) {
                    $("[id$=hdnUploadedImage]").val(obj["name"]);
                    $("[id$=imgprw]").attr('src', rootpath + "uploads/" + $("[id$=hdnUploadedImage]").val())
                    $("[id$=ocr-sec]").show();
                });
            })

            $('#file_BrandImage').on('fileclear', function (event) {
                $("[id$=hdnUploadedImage]").val('');
                $("[id$=ocr-sec]").hide();
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
            <div class="container">
                <!-- Brand and toggle get grouped for better mobile display -->
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">OCR APP</a>
                </div>
                <!-- Collect the nav links, forms, and other content for toggling -->
                <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                    <ul class="nav navbar-nav">
                    </ul>
                </div>
                <!-- /.navbar-collapse -->
            </div>
            <!-- /.container -->
        </nav>

        <!-- Page Content -->
        <div class="container">
            <div class="row">
                <h2>
                    <label>Upload File</label></h2>
                <div class="col-sm-12 text-center">
                    <input id="file_BrandImage" type="file" data-min-file-count="1">
                </div>
            </div>
            <!-- /.row -->
            <div id="ocr-sec" style="display: none;" class="col-sm-12">
                <div class="row">
                    <h2>
                        <label>Upload File Preview</label></h2>
                    <div class="form-group">
                        <image id="imgprw" src="" class="img-responsive" />
                        <div class="col-sm-12">
                            <br />
                        <asp:Button ID="btnOCRReader" runat="server" Text="Read Image Data" CssClass="btn btn-primary" OnClick="btnOCRReader_Click" /></div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblText" runat="server" Text="Label"></asp:Label>
        </div>
        <!-- /.container -->
        <asp:HiddenField ID="hdnUploadedImage" runat="server" />
    </form>
</body>
</html>
