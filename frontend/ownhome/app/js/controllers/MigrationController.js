/**
 * Created by joannarosedelmar on 24/8/14.
 */
'use strict'

migrationApp.controller('MigrationController',

    function MigrationController($scope){
        $scope.rma = {

            header:{
                rmaNumber:'RMANumber',
                flag:'R3ApprovedFlag',
                runningStatus:'Running Status',
                concurrentStatus:'Concurrent Status',
                item:'Item No.'

            },

            itemCount:0,

            items:[
                {
                    row:0,
                    rma:'RMANumber 1',
                    item: 10,
                    flag:'2',
                    runningStatus:'APR',
                    concurrentStatus:'MA'
                },
                {
                    row:0,
                    rma:'RMANumber 1',
                    item:20,
                    flag:'0',
                    runningStatus:'PAPR',
                    concurrentStatus:'MS'
                },
                {
                    row:0,
                    rma:'RMANumber 2',
                    item:10,
                    flag:'2',
                    runningStatus:'APR',
                    concurrentStatus:'MA+DEFG'
                }
            ],

            selectAll:false

        }


}//end of controller function


);//end of controller