class Card {
    constructor(face, suit) {
        this.suits = {
            S: '\u2660',
            H: '\u2665',
            D: '\u2666',
            C: '\u2663'
        };
        this.faces = ['2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K', 'A'];

        this.face = face;
        this.suit = suit;
    }

    set face(newFace) {
        if (!this.faces.includes(newFace)) {
            throw new Error("Invalid card face: " + newFace);
        }
        this._face = newFace;
    }

    get face() {
        return this._face;
    }

    set suit(newSuit) {
        if (!this.suits.hasOwnProperty(newSuit)) {
            throw new TypeError;
        }
        this._suit = newSuit;
    }

    get suit() {
        return this._suit;
    }

    toString() {
        return this.face + this.suits[this.suit];
    }
}

console.log(new Card("J",'D') + "");