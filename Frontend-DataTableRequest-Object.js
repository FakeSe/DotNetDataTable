var table = $('#id_of_your_table').DataTable({
                    'searchable': true,
                    "ordering": true,
                    "serverSide": true,
                    responsive: true,
                    processing: true,
                      ajax: {
                        "url": "/url_to_your_controller_method",
                        "type": "POST",
                        "data": function (data) {

                            return JSON.stringify({ dtParms : data});
                        }
                    },
                    "columnDefs": [
                        {
                            "targets": 0,
                            "data": "The_name_of_the_column0", //Must be the same as the name of the column in the results that you return
                        },
                        {
                            "targets": 1,
                            "data": "The_name_of_the_column1",
                        }
                    ]
                });
