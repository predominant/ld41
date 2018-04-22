'use strict';

module.exports = function(app) {
    var leaderboard = require('../controllers/leaderboardController.js');

    app.route('/scores')
        .get(leaderboard.list_scores)
        .post(leaderboard.submit_score);

    app.route('/scores/:scoreId')
    .get(leaderboard.read_score)
    .put(leaderboard.update_score)
    .delete(leaderboard.delete_score);
};
