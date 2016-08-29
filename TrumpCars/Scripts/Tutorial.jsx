var GameRoom = React.createClass({
	propTypes: {
		CardData: React.PropTypes.shape({
			Title: React.PropTypes.string,
			ImgSrc: React.PropTypes.string,
			Attributes: React.PropTypes.arrayOf(React.PropTypes.shape({
				name: React.PropTypes.string.isRequired,
				value: React.PropTypes.string.isRequired
			}))
		})
	},
    render: function() {
        return (
            <div className="room">
                <div className="table">
                    <Card {...this.props.CardData} />
                    <div className="compare-card"></div>
                </div>
            </div>
        );
    }
});
var Card = React.createClass({
    propTypes: {
        Title: React.PropTypes.string,
        ImgSrc: React.PropTypes.string,
        Attributes: React.PropTypes.arrayOf(React.PropTypes.shape({
            name: React.PropTypes.string.isRequired,
            value: React.PropTypes.string.isRequired
        }))
    },
    render: function () {
		return (
			<div className="card">
				<div className="card__title">{this.props.Title}</div>
				<div className="card__image"><img src={this.props.ImgSrc} /></div>
				<ul>
					{
						this.props.Attributes.map(function(attribute) {
							return (
								<li className="card__attribute">
									<span className="card__attribute__name">{attribute.name}</span>
									<span className="card__attribute__value">{attribute.value}</span>
								</li>
							);
						})
					}
				</ul>
			</div>
		);
    }
});
var data = {
	Title: 'Audi',
	ImgSrc: 'http://d3lp4xedbqa8a5.cloudfront.net/imagegen/max/ccr/300/-/s3/digital-cougar-assets/whichcar/2015/05/27/SUC15C-1422/SUC15C-1.jpg',
	Attributes: [
		{ name: 'Fuel',  value: '5'}, { name: 'Power',  value: '3'}
	]
};
ReactDOM.render(
    <GameRoom CardData={data} />,
    document.getElementById('content')
);
