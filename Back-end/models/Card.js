const mongoose = require("mongoose");
const Schema = mongoose.Schema;

const CardSchema = new Schema({
  name: {
    type: String,
    required: true,
    maxlength: 24,
  },
  description: {
    type: String,
    maxlength: 120,
  },
  level: {
    type: Number,
  },
  maxlevel: {
    type: Number,
  },
  user: {
    type: Schema.Types.ObjectId,
    ref: "users",
  },
});

module.exports = mongoose.model("cards", CardSchema);
