import json
from tkinter import Tk, Label, Entry, Button, Text, END, messagebox, ttk , Toplevel, Spinbox


class JsonEditorApp:
    def __init__(self, root, window2):
        self.root = root
        self.root.title("Editor de JSON")
        self.window2 = window2
        self.window2.title("Añadir decision")

        # Sección de encabezado
        Label(root, text="Titulo historia:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        self.historia_entry = Entry(root, width=30)
        self.historia_entry.grid(row=0, column=1, padx=5, pady=5)

        Label(root, text="Idioma:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        self.idioma_entry = ttk.Combobox(state="readonly",values=["es_ES", "en_EN"])
        self.idioma_entry.grid(row=1, column=1, padx=5, pady=5)

        Label(root, text="Descripción:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        self.descripcion_entry = Entry(root, width=30)
        self.descripcion_entry.grid(row=2, column=1, padx=5, pady=5)
        
        Label(root, text="Coste:").grid(row=3, column=0, sticky="w", padx=5, pady=5)
        self.coste_entry = Entry(root, width=30)
        self.coste_entry.grid(row=3, column=1, padx=5, pady=5)
        
        Label(root, text="Imagen:").grid(row=4, column=0, sticky="w", padx=5, pady=5)
        self.imagen_entry = Entry(root, width=30)
        self.imagen_entry.grid(row=4, column=1, padx=5, pady=5)

        Label(root, text="Nivel (edad):").grid(row=5, column=0, sticky="w", padx=5, pady=5)
        self.nivel_entry = ttk.Combobox(state="readonly",values=["0-10", "11-14","14-..."])
        self.nivel_entry.grid(row=5, column=1, padx=5, pady=5)

        Button(root, text="Guardar JSON", command=self.save_json).grid(row=6, column=1, padx=5, pady=10)

        #Creamos la ventana2
        Label(window2, text="Descripción decision:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        self.decision_desc_entry = Entry(window2, width=30)
        self.decision_desc_entry.grid(row=0, column=1, padx=5, pady=5)

        Label(window2, text="Imagen:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        self.decision_img_entry = Entry(window2, width=30)
        self.decision_img_entry.grid(row=1, column=1, padx=5, pady=5)

        Label(window2, text="Respuesta 1:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        self.decision_res1_entry = Entry(window2, width=30)
        self.decision_res1_entry.grid(row=2, column=1, padx=5, pady=5)

        Label(window2, text="At1:").grid(row=3, column=0, sticky="w", padx=5, pady=5)
        Label(window2, text="At2:").grid(row=3, column=2, sticky="w", padx=5, pady=5)
        Label(window2, text="At3:").grid(row=3, column=4, sticky="w", padx=5, pady=5)
        Label(window2, text="At4:").grid(row=3, column=6, sticky="w", padx=5, pady=5)
        self.decision_res1_at1 = Spinbox(window2, from_=-10, to=10)
        self.decision_res1_at2 = Spinbox(window2, from_=-10, to=10)
        self.decision_res1_at3 = Spinbox(window2, from_=-10, to=10)
        self.decision_res1_at4 = Spinbox(window2, from_=-10, to=10)
        self.decision_res1_at1.grid(row=3, column=1, padx=5, pady=5)
        self.decision_res1_at2.grid(row=3, column=3, padx=5, pady=5)
        self.decision_res1_at3.grid(row=3, column=5, padx=5, pady=5)
        self.decision_res1_at4.grid(row=3, column=7, padx=5, pady=5)

        Label(window2, text="Respuesta 2:").grid(row=4, column=0, sticky="w", padx=5, pady=5)
        self.decision_res2_entry = Entry(window2, width=30)
        self.decision_res2_entry.grid(row=4, column=1, padx=5, pady=5)

        Label(window2, text="At1:").grid(row=5, column=0, sticky="w", padx=5, pady=5)
        Label(window2, text="At2:").grid(row=5, column=2, sticky="w", padx=5, pady=5)
        Label(window2, text="At3:").grid(row=5, column=4, sticky="w", padx=5, pady=5)
        Label(window2, text="At4:").grid(row=5, column=6, sticky="w", padx=5, pady=5)
        self.decision_res2_at1 = Spinbox(window2, from_=-10, to=10)
        self.decision_res2_at2 = Spinbox(window2, from_=-10, to=10)
        self.decision_res2_at3 = Spinbox(window2, from_=-10, to=10)
        self.decision_res2_at4 = Spinbox(window2, from_=-10, to=10)
        self.decision_res2_at1.grid(row=5, column=1, padx=5, pady=5)
        self.decision_res2_at2.grid(row=5, column=3, padx=5, pady=5)
        self.decision_res2_at3.grid(row=5, column=5, padx=5, pady=5)
        self.decision_res2_at4.grid(row=5, column=7, padx=5, pady=5)

        Button(window2, text="Finalizar", command=self.close).grid(row=6, column=1, padx=5, pady=10)
        Button(window2, text="Añadir nueva decisión", command= lambda: self.agregar_decision("Guerrero_ES.json")).grid(row=6, column=2, padx=5, pady=10)

        # Crear JSON inicial vacío
        #self.create_empty_json()
    def close(self):
        exit(0)

    def create_empty_json(self):
        """Crea un JSON vacío con el formato inicial y lo muestra en los campos."""
        # JSON vacío inicial
        data = {
            "historia": "",
            "idioma": "",
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
            if self.historia_entry.get()=="" or self.descripcion_entry.get()=="" or self.imagen_entry.get()=="" or self.coste_entry.get()=="" or self.idioma_entry.get()=="" or self.nivel_entry.get()=="":
                raise Exception("Todos los campos deben estar rellenos")
            # Recoger datos de los campos
            data_historia = {
                "historia": self.historia_entry.get(),
                "desc": self.descripcion_entry.get(),
                "coste": self.coste_entry.get(),
                "imagen": self.imagen_entry.get()
            }

            idioma=self.idioma_entry.get()[-2:]
            self.agregar_historia(f"Historias_{idioma}.json",data_historia)
            self.crear_json_decisiones()

            changeWindow()
            #messagebox.showinfo("Éxito", "Archivo JSON guardado correctamente.")

        except Exception as e:
            messagebox.showerror("Error", f"No se pudo guardar el archivo: {e}")
        
    def agregar_historia(self,archivo, nueva_historia):
        # Leer el archivo JSON
        with open(archivo, 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        # Agregar la nueva historia
        data['historias'].append(nueva_historia)
        
        # Incrementar el contador de historias
        data['num_historias'] = len(data['historias'])
        
        # Guardar los cambios en el archivo
        with open(archivo, 'w', encoding='utf-8') as f:
            json.dump(data, f, indent=4, ensure_ascii=False)

    def crear_json_decisiones(self):
        if self.nivel_entry.get()=="0-10": 
            nivel=1
        if self.nivel_entry.get()=="11-14" :
            nivel=2
        if self.nivel_entry.get()=="14-...":
            nivel=3
        
        datos = {
            "historia": self.historia_entry.get(),
            "idioma": self.idioma_entry.get(),
            "nivel": nivel,
            "decisiones": [],
            "decisiones_respuesta": []
        }
        with open(f"{self.historia_entry.get()}.json", "w", encoding="utf-8") as archivo:
            json.dump(datos, archivo, ensure_ascii=False, indent=4)

    def agregar_decision(self,archivo):
        # Leer el archivo JSON
        with open(archivo, 'r', encoding='utf-8') as f:
            datos = json.load(f)
        
        decision = {
            "id": len(datos["decisiones"]) + 1,
            "imagen": self.decision_img_entry.get(),
            "desc": self.decision_desc_entry.get(),
            "res_der": {
                "respuesta": self.decision_res1_entry.get(),
                "efectos": [
                    self.decision_res1_at1.get(),
                    self.decision_res1_at2.get(),
                    self.decision_res1_at3.get(),
                    self.decision_res1_at4.get()
                ],
                "siguiente": None
            },
            "res_izq": {
                "respuesta": self.decision_res2_entry.get(),
                "efectos": [
                    self.decision_res2_at1.get(),
                    self.decision_res2_at2.get(),
                    self.decision_res2_at3.get(),
                    self.decision_res2_at4.get()
                ],
                "siguiente": None
            }
        }
        print(decision)
        datos["decisiones"].append(decision)
        with open(archivo, 'w', encoding='utf-8') as f:
            json.dump(datos, f, indent=4, ensure_ascii=False)
        
def changeWindow():
    window2.deiconify()  # Muestra la ventana2

if __name__ == "__main__":
    root = Tk()
    window2 = Toplevel()
    window2.withdraw() 
    app = JsonEditorApp(root,window2)
    root.mainloop()




