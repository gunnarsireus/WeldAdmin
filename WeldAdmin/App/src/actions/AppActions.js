import AppDispatcher from '../dispatcher/AppDispatcher';
import AppConstants from '../constants/AppConstants';

let AppActions = {
    add: function (reactionToAdd) {
        AppDispatcher.dispatch({
            actionType: AppConstants.REACTION_ADD,
            data: reactionToAdd
        });
    },
    remove: function (id) {
        AppDispatcher.dispatch({
            actionType: AppConstants.REACTION_REMOVE,
            id: id
        });
    }
}

export default AppActions;