const requestMoviesListType = 'REQUEST_MOVIES';
const receiveMoviesListType = 'RECEIVE_MOVIES';
const resetMovieListError = 'RESET_ERROR';
const initialState = { movies: [], isLoading: false, errorMessage: null };

export const actionCreators = {
    requestMoviesList: () => async (dispatch, getState) => {
        dispatch({ type: requestMoviesListType });

        const url = `api/movie/movies`;
        const response = await fetch(url);
        const movies = await response.json();
        dispatch({ type: receiveMoviesListType, movies });
    },

    resetError: () => async (dispatch, getState) => {
        dispatch({ type: resetMovieListError });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestMoviesListType) {
        return {
            ...state,
            isLoading: true
        };
    }

    if (action.type === receiveMoviesListType) {
        return {
            ...state,
            movies: action.movies.code !== 200 ? [] : action.movies.data,
            isLoading: false,
            errorMessage: action.movies.code !== 200 ? action.movies.message : null
        };
    }

    if (action.type === resetMovieListError) {
        return {
            ...state,
            errorMessage: null
        };
    }

    return state;
};
