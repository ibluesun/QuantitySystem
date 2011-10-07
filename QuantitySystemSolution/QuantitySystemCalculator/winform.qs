
s = new ::System:Windows:Forms:Form
l = new ::System:Windows:Forms:Label

l->Text = "Hello There"
l->top = 60
l->left = 40
s->Controls->Add(l)
s->Text = "Hello There"

s->ShowDialog()
