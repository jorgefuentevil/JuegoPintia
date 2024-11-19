import json
from tkinter import Tk, Label, Entry, Button, Text, END, messagebox, ttk


class JsonEditorApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Editor de JSON")

        # Sección de encabezado
        Label(root, text="Historia:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        self.historia_entry = Entry(root, width=30)
        self.historia_entry.grid(row=0, column=1, padx=5, pady=5)

        Label(root, text="Idioma:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        self.idioma_entry = ttk.Combobox(state="readonly",values=["es_ES", "en_EN"])
        self.idioma_entry.grid(row=1, column=1, padx=5, pady=5)

        Label(root, text="Nivel:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        self.nivel_entry = Entry(root, width=30)
        self.nivel_entry.grid(row=2, column=1, padx=5, pady=5)

        # Sección de decisiones
        Label(root, text="Decisiones:").grid(row=3, column=0, sticky="nw", padx=5, pady=5)
        self.decisiones_text = Text(root, width=60, height=15)
        self.decisiones_text.grid(row=3, column=1, padx=5, pady=5)

        # Botones
        Button(root, text="Crear JSON nuevo", command=self.create_empty_json).grid(row=4, column=0, padx=5, pady=10)
        Button(root, text="Guardar JSON", command=self.save_json).grid(row=4, column=1, padx=5, pady=10)

        # Crear JSON inicial vacío
        self.create_empty_json()

    def create_empty_json(self):
        """Crea un JSON vacío con el formato inicial y lo muestra en los campos."""
        # JSON vacío inicial
        data = {
            "historia": "",
            "idioma": "es_ES",
            "nivel": 1,
            "decisiones": [],
            "decisiones_respuesta": []
        }

        # Rellenar los campos
        self.historia_entry.delete(0, END)
        self.historia_entry.insert(0, data["historia"])

        self.idioma_entry.delete(0, END)
        self.idioma_entry.insert(0, data["idioma"])

        self.nivel_entry.delete(0, END)
        self.nivel_entry.insert(0, str(data["nivel"]))

        self.decisiones_text.delete(1.0, END)
        self.decisiones_text.insert(1.0, json.dumps(data["decisiones"], indent=4, ensure_ascii=False))

        messagebox.showinfo("Nuevo JSON", "Se ha iniciado un nuevo JSON vacío.")

    def save_json(self):
        try:
            # Recoger datos de los campos
            data = {
                "historia": self.historia_entry.get(),
                "idioma": self.idioma_entry.get(),
                "nivel": int(self.nivel_entry.get()),
                "decisiones": json.loads(self.decisiones_text.get(1.0, END).strip())
            }

            with open("datos.json", "w", encoding="utf-8") as file:
                json.dump(data, file, indent=4, ensure_ascii=False)

            messagebox.showinfo("Éxito", "Archivo JSON guardado correctamente.")
        except Exception as e:
            messagebox.showerror("Error", f"No se pudo guardar el archivo: {e}")


if __name__ == "__main__":
    root = Tk()
    app = JsonEditorApp(root)
    root.mainloop()
