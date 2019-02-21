app.config(function ($locationProvider, $httpProvider, $stateProvider) {
    $locationProvider.html5Mode(false);
    var states = [
        {
            name: 'match',
            url: '/match',
            component: 'lista'
        },
        {
            name: 'otherwise',
            url: '*path',
            component: 'lista'
        },
    ]

    states.forEach(function (state) {
        $stateProvider.state(state);
    });
})
