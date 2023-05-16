$(document).ready(function () {


    $("div.container-fluid").LoadingOverlay("show");

    fetch("/DashBoard/GetResumen")
        .then(response => {
            $("div.container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.state) {
                let d = responseJson.object


                $("#totalVenta").text(d.totalSales)
                $("#totalIngresos").text(d.totalIncomes)
                $("#totalProductos").text(d.totalProducts)
                $("#totalCategorias").text(d.totalCategories)

                let barCharLabels;
                let barChartData;

                if (d.saleWeek.length > 0) {
                    barCharLabels = d.saleWeek.map((item) => { return item.date })
                    barChartData = d.saleWeek.map((item) => { return item.total })

                } else {
                    barCharLabels = ["sin resultados"]
                    barChartData = [0]
                }

                let pieCharLabels;
                let pieChartData;

                if (d.productWeek.length > 0) {
                    pieCharLabels = d.productWeek.map((item) => { return item.product })
                    pieChartData = d.productWeek.map((item) => { return item.quantity })

                } else {
                    pieCharLabels = ["sin resultados"]
                    pieChartData = [0]
                }

                let controlVenta = document.getElementById("charVentas");
                let myBarChart = new Chart(controlVenta, {
                    type: 'bar',
                    data: {
                        labels: barCharLabels,
                        datasets: [{
                            label: "Cantidad",
                            backgroundColor: "#4e73df",
                            hoverBackgroundColor: "#2e59d9",
                            borderColor: "#4e73df",
                            data: barChartData,
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: false,
                                    drawBorder: false
                                },
                                maxBarThickness: 50,
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    maxTicksLimit: 5
                                }
                            }],
                        },
                    }
                });

                let controlProducto = document.getElementById("chartProductos");
                let myPieChart = new Chart(controlProducto, {
                    type: 'doughnut',
                    data: {
                        labels: pieCharLabels,
                        datasets: [{
                            data: pieChartData,
                            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: true
                        },
                        cutoutPercentage: 80,
                    },
                });

            }
        })
})