import React from 'react';
import { connect } from 'react-redux';

const Home = props => (
  <div>
    <h1>Welcome to Video Library</h1>
    <p>Go to movies list page</p>
  </div>
);

export default connect()(Home);
