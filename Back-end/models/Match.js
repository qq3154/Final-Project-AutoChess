const mongoose = require("mongoose");
const Schema = mongoose.Schema;

const MatchSchema = new Schema({
  winner: {
    type: Schema.Types.ObjectId,
    ref: "users",
  },
  loser: {
    type: Schema.Types.ObjectId,
    ref: "users",
  },
  round: {
    type: Number,
    required: true,
  },
  createAt: {
    type: Date,
    default: Date.now,
  },
});

module.exports = mongoose.model("matchs", MatchSchema);
