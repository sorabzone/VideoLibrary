import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/MoviesList';

class FetchData extends Component {
    componentWillMount() {
        this.reloadMovies();
    }

    componentWillUnmount() {
        this.props.resetError();
    }

    reloadMovies = () => {
        this.props.requestMoviesList();
    }

    render() {
        return (
            <div>
                <h1>Movies List</h1>
                <p>You will find cheapest price here.</p>
                {renderMoviesTable(this.props)}
                {this.props.errorMessage != null && renderError(this.props.errorMessage)}
                {renderReload(this.props, this.reloadMovies)}
            </div>
        );
    }
}

function renderMoviesTable(props) {
    return (
        <table className='table'>
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Released</th>
                    <th>Genre</th>
                    <th>Director</th>
                    <th>Source</th>
                    <th>Rating</th>
                    <th>Price</th>
                </tr>
            </thead>
            <tbody>
                {props.movies.map(movie =>
                    <tr key={movie.id}>
                        <td>{movie.title}</td>
                        <td>{movie.released}</td>
                        <td>{movie.genre}</td>
                        <td>{movie.director}</td>
                        <td>{movie.id}</td>
                        <td>{movie.rating}</td>
                        <td>{movie.price}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

function renderReload(props, reloadMovies) {
    return <p className='clearfix text-center'>
        <button className='btn btn-default pull-left' onClick={reloadMovies}>Reload List</button>
        {props.isLoading ? <span>Loading...</span> : []}
    </p>;
}

function renderError(errorMessage) {
    return <div className='clearfix text-center'>
            <div><strong>{errorMessage}</strong></div>
        </div>;
}

export default connect(
    state => state.moviesList,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(FetchData);
