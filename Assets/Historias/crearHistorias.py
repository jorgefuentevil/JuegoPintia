import json
from tkinter import Tk, Label, Entry, Button, Text, END, messagebox, ttk , Toplevel, Spinbox

class Decision(Toplevel):
     
    def __init__(self, master = None, app=None, isRespuesta=False):
         
        super().__init__(master = master)
        self.app = app
        self.id_siguiente_izq= None
        self.id_siguiente_der= None
        self.isRespuesta=isRespuesta
        #Creamos la ventana2
        Label(self, text="Creacion de decision seguida de {xxxxxxx}").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        Label(self, text="Descripción decision:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        self.decision_descripcion_entry = Entry(self, width=30)
        self.decision_descripcion_entry.grid(row=1, column=1, padx=5, pady=5)

        Label(self, text="Imagen:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        self.decision_img_entry = Entry(self, width=30)
        self.decision_img_entry.grid(row=2, column=1, padx=5, pady=5)

        Label(self, text="Respuesta 1:").grid(row=3, column=0, sticky="w", padx=5, pady=5)
        self.decision_res1_entry = Entry(self, width=30)
        self.decision_res1_entry.grid(row=3, column=1, padx=5, pady=5)

        Button(self, text="Añadir decision Encadenada", command=self.decision_encadenada_der).grid(row=3, column=2, padx=5, pady=5)

        Label(self, text="At1:").grid(row=4, column=0, sticky="w", padx=5, pady=5)
        Label(self, text="At2:").grid(row=4, column=2, sticky="w", padx=5, pady=5)
        Label(self, text="At3:").grid(row=4, column=4, sticky="w", padx=5, pady=5)
        Label(self, text="At4:").grid(row=4, column=6, sticky="w", padx=5, pady=5)
        self.decision_res1_at1 = Spinbox(self, from_=-10, to=10)
        self.decision_res1_at2 = Spinbox(self, from_=-10, to=10)
        self.decision_res1_at3 = Spinbox(self, from_=-10, to=10)
        self.decision_res1_at4 = Spinbox(self, from_=-10, to=10)
        self.decision_res1_at1.grid(row=4, column=1, padx=5, pady=5)
        self.decision_res1_at2.grid(row=4, column=3, padx=5, pady=5)
        self.decision_res1_at3.grid(row=4, column=5, padx=5, pady=5)
        self.decision_res1_at4.grid(row=4, column=7, padx=5, pady=5)

        Label(self, text="Explicación respuesta 1:").grid(row=5, column=0, sticky="w", padx=5, pady=5)
        self.decision_res1_exp_entry = Entry(self, width=30)
        self.decision_res1_exp_entry.grid(row=5, column=1, padx=5, pady=5)

        Label(self, text="Respuesta 2:").grid(row=6, column=0, sticky="w", padx=5, pady=5)
        self.decision_res2_entry = Entry(self, width=30)
        self.decision_res2_entry.grid(row=6, column=1, padx=5, pady=5)
        
        Button(self, text="Añadir decision Encadenada", command=self.decision_encadenada_izq).grid(row=6, column=2, padx=5, pady=5)

        Label(self, text="At1:").grid(row=7, column=0, sticky="w", padx=5, pady=5)
        Label(self, text="At2:").grid(row=7, column=2, sticky="w", padx=5, pady=5)
        Label(self, text="At3:").grid(row=7, column=4, sticky="w", padx=5, pady=5)
        Label(self, text="At4:").grid(row=7, column=6, sticky="w", padx=5, pady=5)
        self.decision_res2_at1 = Spinbox(self, from_=-10, to=10)
        self.decision_res2_at2 = Spinbox(self, from_=-10, to=10)
        self.decision_res2_at3 = Spinbox(self, from_=-10, to=10)
        self.decision_res2_at4 = Spinbox(self, from_=-10, to=10)
        self.decision_res2_at1.grid(row=7, column=1, padx=5, pady=5)
        self.decision_res2_at2.grid(row=7, column=3, padx=5, pady=5)
        self.decision_res2_at3.grid(row=7, column=5, padx=5, pady=5)
        self.decision_res2_at4.grid(row=7, column=7, padx=5, pady=5)

        Label(self, text="Explicación respuesta 2:").grid(row=8, column=0, sticky="w", padx=5, pady=5)
        self.decision_res2_exp_entry = Entry(self, width=30)
        self.decision_res2_exp_entry.grid(row=8, column=1, padx=5, pady=5)

        Label(self, text="Personaje decision:").grid(row=9, column=0, sticky="w", padx=5, pady=5)
        self.decision_personaje = Entry(self, width=30)
        self.decision_personaje.grid(row=9, column=1, padx=5, pady=5)


        Button(self, text="Finalizar", command=self.close).grid(row=10, column=1, padx=5, pady=10)
        Button(self, text="Añadir nueva decisión", command=self.agregar_decision).grid(row=10, column=2, padx=5, pady=10)

    def close(self):
        exit(0)
        
    def agregar_decision(self):
        try:
            if len(self.decision_descripcion_entry.get())>200:
                raise Exception("La descripción es muy extensa")
            if len(self.decision_res1_entry.get())>200:
                raise Exception("La decision derecha es muy extensa")
            if len(self.decision_res2_entry.get())>200:
                raise Exception("La decision izquierda es muy extensa")
            if self.decision_descripcion_entry.get()=="" or self.decision_res1_entry.get()=="" or self.decision_res2_entry.get()=="" or self.decision_img_entry.get()=="" or self.decision_personaje.get()=="":
                raise Exception("Todos los campos deben de estar rellenos")
            # Leer el archivo JSON
            with open(f"{self.app.titulo}_{self.app.idioma_entry.get()[-2:]}.json", 'r', encoding='utf-8') as f:
                datos = json.load(f)
            if self.isRespuesta == False:
                id=len(datos["decisiones"]) 
            else:
                id=self.app.siguiente

            explicacionizq = None if self.decision_res2_exp_entry.get() == "" else self.decision_res2_exp_entry.get() 
            explicacionder = None if self.decision_res1_exp_entry.get() == "" else self.decision_res1_exp_entry.get()
            decision = {
                "id": id,
                "personaje": self.decision_personaje.get(),
                "imagen": self.decision_img_entry.get(),
                "desc": self.decision_descripcion_entry.get(),
                "res_der": {
                    "respuesta": self.decision_res1_entry.get(),
                    "explicacion": explicacionder,
                    "efectos": [
                        self.decision_res1_at1.get(),
                        self.decision_res1_at2.get(),
                        self.decision_res1_at3.get(),
                        self.decision_res1_at4.get()
                    ],
                    "siguiente": self.id_siguiente_der
                },
                "res_izq": {
                    "respuesta": self.decision_res2_entry.get(),
                    "explicacion": explicacionizq,
                    "efectos": [
                        self.decision_res2_at1.get(),
                        self.decision_res2_at2.get(),
                        self.decision_res2_at3.get(),
                        self.decision_res2_at4.get()
                    ],
                    "siguiente": self.id_siguiente_izq
                }
            }
            if self.isRespuesta == False:
                datos["decisiones"].append(decision)
            else:
                datos["decisiones_respuesta"].append(decision)
                self.app.siguiente+=1
            with open(f"{self.app.titulo}_{self.app.idioma_entry.get()[-2:]}.json", 'w', encoding='utf-8') as f:
                json.dump(datos, f, indent=4, ensure_ascii=False)
            if self.isRespuesta == False:
                self.decision_res1_entry.delete(0,END)
                self.decision_res2_entry.delete(0,END)
                self.decision_img_entry.delete(0,END)
                self.decision_descripcion_entry.delete(0,END)
                self.decision_res1_exp_entry.delete(0,END)
                self.decision_res2_exp_entry.delete(0,END)
                self.id_siguiente_izq=-1
                self.id_siguiente_der=-1
                self.decision_personaje.delete(0,END)


            else:
                self.destroy()
        except Exception as e:
            messagebox.showerror("Error", f"No se pudo guardar el archivo: {e}")
        

    def decision_encadenada_der(self):        
        Decision(self.master,self.app,True)
        self.id_siguiente_der=self.app.siguiente

    def decision_encadenada_izq(self):
        Decision(self.master,self.app,True)
        self.id_siguiente_izq=self.app.siguiente
        

    
class JsonEditorApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Editor de JSON")
        self.repetida=False
        self.titulo=""
        self.siguiente=0
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

        Label(root, text="Atributo Especifico:").grid(row=6, column=0, sticky="w", padx=5, pady=5)
        self.nivel_atributo_especifico = Entry(root, width=30)
        self.nivel_atributo_especifico.grid(row=6, column=1, padx=5, pady=5)

        Button(root, text="Guardar JSON", command=self.save_json).grid(row=7, column=1, padx=5, pady=10)

        # Crear JSON inicial vacío
        #self.create_empty_json()
    

    def create_empty_json(self):
        """Crea un JSON vacío con el formato inicial y lo muestra en los campos."""
        # JSON vacío inicial
        data = {
            "historia": "",
            "idioma": "",
            "nivel": 1,
            "aleatoria": "true",
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
            if len(self.descripcion_entry.get())>200:
                raise Exception("La descripción es muy extensa")
            if len(self.historia_entry.get())>20:
                raise Exception("El titulo es muy extenso")

            data_historia = {
                "personaje": self.historia_entry.get(),
                "desc": self.descripcion_entry.get(),
                "coste": self.coste_entry.get(),
                "imagen": self.imagen_entry.get(),
                "atributo": self.nivel_atributo_especifico.get()
            }
            self.titulo=self.historia_entry.get().replace(" ", "_")
            idioma=self.idioma_entry.get()[-2:]
            self.agregar_historia(f"Historias_{idioma}.json",data_historia)

            if(self.repetida == False):
                self.crear_json_decisiones()
        
            Decision(self.root,self,False)

        except Exception as e:
            messagebox.showerror("Error", f"No se pudo guardar el archivo: {e}")
        
    def agregar_historia(self,archivo, nueva_historia):
        # Leer el archivo JSON
        with open(archivo, 'r', encoding='utf-8') as f:
            data = json.load(f)
        
        for historia in data['historias']:
           if historia['personaje'] == nueva_historia['personaje']:
                print("repe")
                self.repetida=True
                with open(f"{self.titulo}_{self.idioma_entry.get()[-2:]}.json", 'r', encoding='utf-8') as f:
                    datos = json.load(f)
                self.siguiente=len(datos["decisiones_respuesta"])
                return
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
        with open(f"{self.titulo}_{self.idioma_entry.get()[-2:]}.json", "w", encoding="utf-8") as archivo:
            json.dump(datos, archivo, ensure_ascii=False, indent=4)

if __name__ == "__main__":
    root = Tk()
    app = JsonEditorApp(root)
    root.mainloop()




