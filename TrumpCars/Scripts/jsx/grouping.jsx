var GroupContainer = React.createClass({
    getInitialState: function () {
        return {
            groupName: "",
            cards: null
        };
    },
    componentDidMount: function () {
        var self = this;
        var vhub = $.connection.gameHub;
        vhub.client.userJoined = function (name) {
            self.setState({ groupName: name });
        };
        vhub.client.loadGame = function (name, cards) {
            var cardsArray = JSON.parse(cards);
            self.setState({ groupName: name, cards: cardsArray });
        };
        $.connection.hub.start().done(function () {
            vhub.server.joinRoom("xxx");
        });

    },
    render: function() {
        var self = this;
        var data = "";
        if (self.state.cards) {
            data = self.state.cards.map(function(card, index) {
                return (
                    <p>{card}</p>
                );
            });
        }
        return (
            <div>
                <p>{self.state.groupName}</p>
                {data}
            </div>
        );
    }
});

React.render(
    <GroupContainer />,
    document.getElementById('container')
);