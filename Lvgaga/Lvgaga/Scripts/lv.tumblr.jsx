var Tumblr = React.createClass({
  render: function() {
    return (
      <div>
        Hello, world! I am a Tumblr.
      </div>
    );
  }
});
React.render(
  <Tumblr />,
  document.getElementById('div_tumblrs')
);