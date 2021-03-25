<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CpuMonitoring._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.1.6/Chart.bundle.min.js"></script>
    <script>
        var now = new Date();
        var count = 1;
        var label_name = ['CPU', 'Momory'],
            bg_color = ['#fffcd6', '#e3f4ff'],
            border_color = ['#d0c00b', '#2d83be'],
            tmp_data = ['0', '0'];

        var ctx = $('#myChart');

        //$(function () {
        //    //alert("cccc");
        //    $("#myCPU").text("bbbb");
        //});


        var data = {

            labels: ['start', now.getHours() + ':' + (now.getMinutes() < 10 ? '0' : '') + now.getMinutes()],
            datasets: [
                {
                    label: label_name[0],
                    data: [null, tmp_data[0]],
                    fill: false,
                    lineTension: 0,
                    backgroundColor: bg_color[0],
                    borderColor: border_color[0],
                    borderCapStyle: 'butt',
                    borderJoinStyle: 'miter',
                    pointBorderColor: border_color[0],
                    pointBackgroundColor: '#fff',
                    pointBorderWidth: 1,
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: bg_color[0],
                    pointHoverBorderColor: border_color[0],
                    pointHoverBorderWidth: 2,
                    pointHitRadius: 10,
                    spanGaps: false
                },
            ]
        };

        var options = {
            scaleOverride: true,
            scaleSteps: 10,
            scaleStepWidth: 10,
            scaleStartValue: 0,
            scales: {
                yAxes: [{
                    display: false,
                }],
                xAxes: [{
                    gridLines: {
                        show: true,
                        color: '#555',
                    },
                    ticks: {
                        fontColor: '#c5c5c5'
                    }
                }]
            }
        };

        var mychart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: options
        });

        var updateData = function (olddata) {
            var labels = olddata['labels'];
            var datasetA = olddata['datasets'][0]['data'];


            $.ajax({
                type: "POST",
                url: "WebService1.asmx/getCPU",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var cpuUsage = response.d;
                    var cpuUsage2 = Math.floor(cpuUsage);

                    now = new Date();
                    labels.push(now.getHours() + ':' + (now.getMinutes() < 10 ? '0' : '') + now.getMinutes());
                    datasetA.push(cpuUsage2);

                    $("#myCPU").text(cpuUsage2);

                },
                failure: function (response) {
                    alert(response.d);
                }
            });

            if (count > 8) {
                labels.shift();
                datasetA.shift();
            }

            count++;
        }

        //setInterval(function () {
        //    alert('1');
        //}, 5000);

        $(function () {
            alert("cccc");
            //setInterval(function () {
            //    alert("cccc");
            //    updateData(data);
            //    mychart.update();
            //}, 1000);
        });

    </script>

    <div id="myCPU">aaaa</div>

    <canvas id="myChart" width="400" height="400"></canvas>
    <script>
        //var ctx = document.getElementById("myChart");
        //var myChart = new Chart(ctx, {
        //    type: 'bar',
        //    data: {
        //        labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
        //        datasets: [{
        //            label: '# of Votes',
        //            data: [12, 19, 3, 5, 2, 3],
        //            backgroundColor: [
        //                'rgba(255, 99, 132, 0.2)',
        //                'rgba(54, 162, 235, 0.2)',
        //                'rgba(255, 206, 86, 0.2)',
        //                'rgba(75, 192, 192, 0.2)',
        //                'rgba(153, 102, 255, 0.2)',
        //                'rgba(255, 159, 64, 0.2)'
        //            ],
        //            borderColor: [
        //                'rgba(255,99,132,1)',
        //                'rgba(54, 162, 235, 1)',
        //                'rgba(255, 206, 86, 1)',
        //                'rgba(75, 192, 192, 1)',
        //                'rgba(153, 102, 255, 1)',
        //                'rgba(255, 159, 64, 1)'
        //            ],
        //            borderWidth: 1
        //        }]
        //    },
        //    options: {
        //        scales: {
        //            yAxes: [{
        //                ticks: {
        //                    beginAtZero:true
        //                }
        //            }]
        //        }
        //    }
        //});

    </script>

</asp:Content>

