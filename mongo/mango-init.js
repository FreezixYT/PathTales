// Ce modèle représente les utilisateurs (visiteurs authentifiés et administrateurs).
const mongoose = require('mongoose');
const Schema = mongoose.Schema;

const UserSchema = new Schema({
  name: { type: String, required: true, trim: true },
  email: { type: String, required: true, unique: true, trim: true },
  password: { type: String, required: true },
  role: { type: String, enum: ['member', 'admin'], default: 'member' },
  isBlocked: { type: Boolean, default: false },
  blockReason: { type: String, default: null },
}, {
  timestamps: true,
});

module.exports = mongoose.model('User', UserSchema);

// Ce modèle gère les histoires interactives créées par les utilisateurs.

const StorySchema = new Schema({
  title: { type: String, required: true, trim: true, maxlength: 150 },
  description: { type: String, trim: true, maxlength: 500 },
  paragraphs: [{ type: Schema.Types.ObjectId, ref: 'Paragraph' }],
  userId: { type: Schema.Types.ObjectId, ref: 'User', required: true },
  status: { type: String, enum: ['published', 'hidden', 'deleted'], default: 'published' },
  categories: [{ type: String }],
  readCount: { type: Number, default: 0 },  
  ratings: [{
    userId: { type: Schema.Types.ObjectId, ref: 'User', required: true },
    score: { type: Number, min: 1, max: 5, required: true },
  }],
}, {
  timestamps: true,
});

module.exports = mongoose.model('Story', StorySchema);

//Ce modèle représente les paragraphes d'une histoire.
const ChoiceSchema = require('./ChoiceSchema');

const ParagraphSchema = new Schema({
  text: { type: String, required: true, trim: true, maxlength: 1000 },
  // La liste des choix proposés à la fin de ce paragraphe
  choices: {
    type: [ChoiceSchema], // Chaque choix suit la structure définie dans le fichier "ChoiceSchema"
    default: [], // Par défaut, un paragraphe n’a pas de choix

    // Validation personnalisée des choix selon que le paragraphe est une fin ou pas
    validate: {
      validator: function (val) {
        // Si le paragraphe est une fin (victory, defeat ou neutral)
        if (this.endingType) {
          return val.length === 0; // Alors il ne doit contenir aucun choix
        } else {
          // Sinon (paragraphe intermédiaire), il doit avoir entre 2 et 4 choix
          return val.length >= 2 && val.length <= 4;
        }
      },
      // Message d’erreur affiché si la validation échoue
      message: props => {
        return props.value.length === 0
          ? 'Un paragraphe sans fin doit contenir entre 2 et 4 choix.'
          : 'Un paragraphe de fin ne doit contenir aucun choix.';
      }
    }
  },
  endingType: { type: String, enum: ['victory', 'defeat', 'neutral'], default: null },
});
module.exports = mongoose.model('Paragraph', ParagraphSchema);

//Sous-schéma pour les choix des paragraphes.

const ChoiceSchema = new Schema({
  text: { type: String, required: true, trim: true, maxlength: 200 },
  nextParagraphId: { type: Schema.Types.ObjectId, ref: 'Paragraph', required: true },
});

module.exports = ChoiceSchema;

const ReportSchema = new Schema({
  storyId: { type: Schema.Types.ObjectId, ref: 'Story', required: true },
  reportedBy: { type: Schema.Types.ObjectId, ref: 'User', required: true },
  reason: { type: String, required: true, maxlength: 500 },
  status: { type: String, enum: ['pending', 'resolved'], default: 'pending' },
}, {
  timestamps: true,
});

module.exports = mongoose.model('Report', ReportSchema);
