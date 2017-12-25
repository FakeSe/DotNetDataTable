# DotNetDataTable : This guide is for very newbies ^^
This classes helps you to implement a .net code which render a datatable in server side, with the pagination, order and all it features.

How to use :
1- First, add DataTablesRequest.cs to your project, don't forget to change the namespace as you wish.

2- Now, in your controller add this notation before you function : 
      [HttpPost("/A_Custom_route_name")] or just  [HttpPost]
 
3- The first thing is to extract the parameters from the datatable request and for that we have 2 options :

    A/ Take your request as a string then convert it to JSon :
        Example :
             [HttpPost]
             public JsonResult RenderDataTable(string req)
             {
                 var dtParms = req.FromJson<DataTablesRequest>();
                 ...
              }
         To use this option, consider installing the Newtonsoft package, in package manager write this : 
              Install-Package Newtonsoft.Json -Version 10.0.3
           
     in this case, in the frontend, your code must look like this :
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

                                return "req=" + JSON.stringify(data);
                            }
                        },
                        "columnDefs": [
                            {
                                "targets": 0,
                                "data": "The_name_of_the_column0", //Must be the same as the name in the results that you return
                            },
                            {
                                "targets": 1,
                                "data": "The_name_of_the_column1",
                            }
                        ]
                    });
      B/ Directly take your request as a DataTableRequest Object :
           Example :
             [HttpPost]
             public JsonResult RenderDataTable( [FromBody] DataTablesRequest dtParms)
             {
                //You already have your dtParms so just use it
              }
          In this case, you don't need to install any other package, just in the frontend when you declare the datatable your ajax call           will look like this : 
                 ajax: {
                            "url": "/url_to_your_controller_method",
                            "type": "POST",
                            "data": function (data) {

                                return JSON.stringify({ dtParms : data});
                            }
                            
4- now let's implement the rest, supposing that you got your dtParms, your function should look like this :
      
      [HttpPost]
        public JsonResult RenderDataTable( [FromBody] DataTablesRequest dtParms)
        {
        //Get all your data, we need it to return the total count to dataTable
            var toQuery = _dbContext.YourTable.AsNoTracking().Where(f => true);
            
        //The TotalCount is required from DataTable so before filtring our records we place it somewhere
            var totalCount = toQuery.Count();
            
        //Now let's define/get our records that we need
            var dataPage = toQuery;

        //First we check if there is a search filter, if yes we filter our results :
            if (!string.IsNullOrEmpty(dtParms.Search.Value))
            {
                dataPage = dataPage.Where(u => u.The_Column_That_We_Will_Filter.ToUpper().Contains(dtParms.Search.Value));
                //You can add as columns as you want
            }
        //Then we check if the datatable require a sorting for data :
            if (dtParms.Order != null)
            { 
            //If true, we get the sorting informations (which column and direction "ASC" or "DESC"
                var requestedOrder = dtParms.Order.FirstOrDefault();
             //And based on those informations we sort our data:
                switch (requestedOrder.Column)
                {
                //if the datatable requested a sorting on the column 0 you sort that column in your records:
                    case 0: 
                        dataPage = requestedOrder.Dir.ToUpper().Equals("DESC") ?  
                                 dataPage.OrderByDescending(f => f.The_column_that_corresponding_the_column0_in_your_datatable)                                         : dataPage.OrderBy(f => f.The_column_that_corresponding_the_column0_in_your_datatable);
                        break;
                    case 1:
                        dataPage = requestedOrder.Dir.ToUpper().Equals("DESC") ? 
                                  dataPage.OrderByDescending(f => f.The_column_that_corresponding_the_column1_in_your_datatable) 
                                   : dataPage.OrderBy(f => f.The_column_that_corresponding_the_column1_in_your_datatable);
                        break;
                }
            }
            
        //Get the results :
            var results = dataPage.Skip(dtParms.Start).Take(dtParms.Length).Select(a => new Your_Model_or_object
            {
                Column0 = a.YourDBTable.Column_that_corresponding_Column0,
                Column1 = a.YourDBTable.Column_that_corresponding_Column0
        }).ToList();
        //The filtredCount is how much records will you return, DataTable require this informations to create the pagination
            var filteredCount = dataPage.Count();

        //This will create the response desired by DataTable
            var res = DataTableResponse.Create(dtParms, totalCount, filteredCount, results);

            return new JsonResult(res);
        }
          
